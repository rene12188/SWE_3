using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SWE3.Demo.OrmFramework.MetaModel;



namespace SWE3.Demo.OrmFramework
{
    /// <summary>This class allows access to OR framework functionalities.</summary>
    public static class Orm
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private static members                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Entities.</summary>
        private static Dictionary<Type, __Entity> _Entities = new Dictionary<Type, __Entity>();



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static properties                                                                                         //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets or sets the database connection used by the framework.</summary>
        public static IDbConnection Connection { get; set; }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static methods                                                                                            //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets an object.</summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="pk">Primary key.</param>
        /// <returns>Object.</returns>
        public static T Get<T>(object pk)
        {
            return (T) _CreateObject(typeof(T), pk);
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private static methods                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets an entity descriptor for an object.</summary>
        /// <param name="o">Object.</param>
        /// <returns>Entity.</returns>
        internal static __Entity _GetEntity(this object o)
        {
            Type t = ((o is Type) ? (Type) o :o.GetType());

            if(!_Entities.ContainsKey(t))
            {
                _Entities.Add(t, new __Entity(t));
            }

            return _Entities[t];
        }


        /// <summary>Saves an object.</summary>
        /// <param name="obj">Object.</param>
        public static void Save(object obj)
        {
            __Entity ent = obj._GetEntity();

            IDbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = ("INSERT INTO " + ent.TableName + " (");

            string update = "ON CONFLICT (" + ent.PrimaryKey.ColumnName + ") DO UPDATE SET ";
            string insert = "";

            IDataParameter p;
            bool first = true;
            for(int i = 0; i < ent.Fields.Length; i++)
            {
                if(i > 0) { cmd.CommandText += ", "; insert += ", "; }
                cmd.CommandText += ent.Fields[i].ColumnName;

                insert += (":v" + i.ToString());

                p = cmd.CreateParameter();
                p.ParameterName = (":v" + i.ToString());
                p.Value = ent.Fields[i].ToColumnType(ent.Fields[i].GetValue(obj));
                cmd.Parameters.Add(p);

                if(!ent.Fields[i].IsPrimaryKey)
                {
                    if(first) { first = false; } else { update += ", "; }
                    update += (ent.Fields[i].ColumnName + " = " + (":w" + i.ToString()));

                    p = cmd.CreateParameter();
                    p.ParameterName = (":w" + i.ToString());
                    p.Value = ent.Fields[i].ToColumnType(ent.Fields[i].GetValue(obj));
                    cmd.Parameters.Add(p);
                }
            }
            cmd.CommandText += (") VALUES (" + insert + ") " + update);

            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }



        /// <summary>Creates an object from a database reader.</summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="re">Reader.</param>
        /// <returns>Object.</returns>
        private static object _CreateObject(Type t, IDataReader re)
        {
            object rval = Activator.CreateInstance(t);
            
            foreach(__Field i in t._GetEntity().Fields)
            {
                i.SetValue(rval, i.ToFieldType(re.GetValue(re.GetOrdinal(i.ColumnName))));
            }

            return rval;
        }


        /// <summary>Creates an instance by its primary keys.</summary>
        /// <param name="t">Type.</param>
        /// <param name="pk">Primary key.</param>
        /// <returns>Object.</returns>
        private static object _CreateObject(Type t, object pk)
        {
            IDbCommand cmd = Connection.CreateCommand();

            cmd.CommandText = t._GetEntity().GetSQL() + " WHERE " + t._GetEntity().PrimaryKey.ColumnName + " = :pk";
            
            IDataParameter p = cmd.CreateParameter();
            p.ParameterName = (":pk");
            p.Value = pk;
            cmd.Parameters.Add(p);

            object rval = null;
            IDataReader re = cmd.ExecuteReader();
            if(re.Read())
            {
                rval = _CreateObject(t, re);
            }

            re.Close();
            cmd.Dispose();

            if(rval == null) { throw new Exception("No data."); }
            return rval;
        }
    }
}
