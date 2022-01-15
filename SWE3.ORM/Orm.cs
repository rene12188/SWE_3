using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Microsoft.Data.Sqlite;
using Npgsql;
using SWE3.ORM.MetaModel;

namespace SWE3.ORM
{
    public static class Orm
    {
        public  enum DBTYPE
        {
            Postgres = 0,
            SQLite = 1
        }

        public static DBTYPE Target = DBTYPE.Postgres;
     
        private static Dictionary<Type, __Entity> _Entities = new Dictionary<Type, __Entity>();
        
        public static string Connectionstring;


        //Generates a DB Connection with the Connectionstring
        public static IDbConnection Connection
        {
            get
            {
                if (Connectionstring == null)
                {
                    throw new NullReferenceException("Please Fill Connectionstring");
                }

                if (Target == DBTYPE.Postgres)
                {
                    var tmp = new NpgsqlConnection(Connectionstring);
                    tmp.Open();
                    return tmp;
                }
                else
                {
                    var tmp = new SQLiteConnection(Connectionstring);
                    tmp.Open();
                    return tmp;
                }
               


            }
            private set
            {
                return;
                

            }
        }

        public static ICache Cache { get; set; }


        public static ILocking Locking { get; set; }


        //Get an Object with a certain PK
        public static T Get<T>(object pk)
        {
            return (T) InitObject(typeof(T), pk);
        }


        public static Query<T> From<T>()
        {
            return new Query<T>(null);
        }

        //Locks 
        public static void Lock(object obj)
        {
            if(Locking != null) { Locking.Lock(obj); }
        }
         //Releases the Lock
        public static void Release(object obj)
        {
            if(Locking != null) { Locking.Release(obj); }
        }

        //Saves an Object
        public static void Save(object obj)
        {
            __Entity ent = obj._GetEntity();
            __Entity bse = obj.GetType().BaseType._GetEntity();

            if(bse.IsMaterial) { _Save(obj, bse, false, true); }
            _Save(obj, ent, bse.IsMaterial, false);
        }

        //Deletes an Object from the Database
        public static void Delete(object obj)
        {
            __Entity ent = obj._GetEntity();
            __Entity bse = obj.GetType().BaseType._GetEntity();

            if(bse.IsMaterial)
            {
                _Delete(obj, ent, false);
                _Delete(obj, bse, true);
            }
            else { _Delete(obj, ent, true); }
        }

        internal static __Entity _GetEntity(this object o)
        {
            Type t = ((o is Type) ? (Type) o :o.GetType());

            if(!_Entities.ContainsKey(t))
            {
                _Entities.Add(t, new __Entity(t));
            }

            return _Entities[t];
        }

        //Get all Child Types of an Object
        internal static Type[] _GetChildTypes(this Type t)
        {
            List<Type> rval = new List<Type>();
            foreach(Type i in _Entities.Keys)
            {
                if(t.IsAssignableFrom(i) && (!i.IsAbstract)) { rval.Add(i); }
            }

            return rval.ToArray();
        }

        //Search Cache for a certain PK
        internal static object _SearchCache(Type t, object pk)
        {
            if((Cache != null) && Cache.Contains(t, pk))
            {
                return Cache.Get(t, pk);
            }

          

            return null;
        }


        //Initilizes Object when loaded from the DB
        public static object InitObject(Type type, object primaryKey)
        {
            object resultValue = Cache.Get(type, primaryKey);

            if (resultValue == null)
            {
                IDbCommand command = Connection.CreateCommand();
                __Entity modelEntity = type._GetEntity();
                command.CommandText = modelEntity.GetSQL() + " WHERE " + modelEntity.PrimaryKey.ColumnName + " = :pk";

                IDataParameter para = command.CreateParameter();
                para.ParameterName = (":pk");
                para.Value = primaryKey;
                command.Parameters.Add(para);

                IDataReader readerData = command.ExecuteReader();
                Dictionary<string, object> columnValuePairs = DataReaderToDictionary(readerData, modelEntity);
                readerData.Close();
                resultValue = InitObject(type, columnValuePairs);
                command.Dispose();
            }
            if (resultValue == null) { throw new Exception("No data."); }
          
            return resultValue;
        }


        //Utility Function to Convert a Data Reader to a Dictionary 
        private static Dictionary<string, object> DataReaderToDictionary(IDataReader dataReader, __Entity entity)
        {
            Dictionary<string, object> columnValuePairs = new();
            if (dataReader.Read())
            {
                foreach (__Field modelField in entity.Internals)
                {
                    columnValuePairs.Add(modelField.ColumnName, dataReader.GetValue(dataReader.GetOrdinal(modelField.ColumnName)));
                }
            }
            return columnValuePairs;
        }


        public static object InitObject(Type type, Dictionary<string, object> columnValuePairs)
        {
            __Entity modelEntity = type._GetEntity();
            object resultValue = _SearchCache(type, modelEntity.PrimaryKey.ToFieldType(columnValuePairs[modelEntity.PrimaryKey.ColumnName], Cache));
            bool foundInChache = true;
            if (resultValue == null)
            {
                foundInChache = false;
                resultValue = Activator.CreateInstance(type);
                
            }

            foreach (__Field inField in modelEntity.Internals)
            {
                inField.SetValue(resultValue, inField.ToFieldType(columnValuePairs[inField.ColumnName], Cache));
            }

            Cache.Put((resultValue));



            return resultValue;
        }
            
      

    internal static void _FillList(Type t, object list, List<Dictionary<string,object>> re)
        {
            foreach (var dict in re)
            {
                list.GetType().GetMethod("Add").Invoke(list, new object[] { InitObject(t, dict) });
            }
        }



    internal static void _FillList(Type t, object list, string sql, IEnumerable<Tuple<string, object>> parameters)
    {
            IDbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = sql;

            foreach (Tuple<string, object> i in parameters)
            {
                IDataParameter p = cmd.CreateParameter();
                p.ParameterName = i.Item1;
                p.Value = i.Item2;
                if (p.Value == null)
                    p.Value = DBNull.Value;
                cmd.Parameters.Add(p);
            }
            __Entity modelEntity = t._GetEntity();

            IDataReader readerData = cmd.ExecuteReader();

            List<Dictionary<string, object>> tempList = new();
            Dictionary<string, object> columnValuePairs = null;

            do
            {
                columnValuePairs = DataReaderToDictionary(readerData, modelEntity);
                if (columnValuePairs.Count > 0)
                    tempList.Add(columnValuePairs);
            }
            while (columnValuePairs != null && columnValuePairs.Count > 0);
            readerData.Close();

            _FillList(t, list, tempList);
            readerData.Dispose();
            cmd.Dispose();
        }


        private static void _Delete(object obj, __Entity ent, bool isBase)
        {
            IDbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = ("DELETE FROM " + ent.TableName + " WHERE " + (isBase ? ent.PrimaryKey.ColumnName : ent.ChildKey) + " = :pk");
            IDataParameter p = cmd.CreateParameter();
            p.ParameterName = ":pk";
            p.Value = ent.PrimaryKey.GetValue(obj);
            cmd.Parameters.Add(p);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            if(Cache != null) { Cache.Remove(obj); }
        }


        private static void _Save(object obj, __Entity ent, bool materialBased, bool isBase)
        {
            if(Cache != null) { if(!Cache.HasChanged(obj)) return; }

            IDbCommand cmd = Connection.CreateCommand();
            string update = "";
            string insert = "";
            cmd.CommandText = ("INSERT INTO " + ent.TableName + " (");
            if(materialBased)
            {
                cmd.CommandText += ent.ChildKey + ", ";
                update = "ON CONFLICT (" + ent.ChildKey + ") DO UPDATE SET ";
                insert = (":ck, ");

                IDataParameter k = cmd.CreateParameter();
                k.ParameterName = ":ck";
                k.Value = ent.PrimaryKey.GetValue(obj);
                cmd.Parameters.Add(k);
            }
            else
            {
                update = "ON CONFLICT (" + ent.PrimaryKey.ColumnName + ") DO UPDATE SET ";
            }


            IDataParameter p;
            bool first = true;
            for(int i = 0; i < ent.LocalInternals.Length; i++)
            {
                if(i > 0) { cmd.CommandText += ", "; insert += ", "; }
                cmd.CommandText += ent.LocalInternals[i].ColumnName;

                insert += (":v" + i.ToString());
              
                p = cmd.CreateParameter();
                p.ParameterName = (":v" + i.ToString());
                p.Value = ent.LocalInternals[i].ToColumnType(ent.LocalInternals[i].GetValue(obj));
                if (p.Value == null)
                {
                    p.Value = DBNull.Value;
                }
                cmd.Parameters.Add(p);

                if(!ent.LocalInternals[i].IsPrimaryKey)
                {
                    if(first) { first = false; } else { update += ", "; }
                    update += (ent.LocalInternals[i].ColumnName + " = " + (":w" + i.ToString()));

                    p = cmd.CreateParameter();
                    p.ParameterName = (":w" + i.ToString());
                    p.Value = ent.LocalInternals[i].ToColumnType(ent.LocalInternals[i].GetValue(obj));
                    if (p.Value == null)
                    {
                        p.Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(p);
                }
            }
            cmd.CommandText += (") VALUES (" + insert + ") " + update);

            cmd.ExecuteNonQuery();
            cmd.Dispose();

            if(!isBase) foreach(__Field i in ent.Externals) { i.UpdateReferences(obj); }

            if(Cache != null) { Cache.Put(obj); }
        }

        
    }
}
