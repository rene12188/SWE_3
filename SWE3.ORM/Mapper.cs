using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using SWE3.ORM.Attributes;
using SWE3.ORM.MetaModel;

namespace SWE3.ORM
{
    public static class Mapper
    {

        private static Dictionary<Type, __Entity> _Entities = new Dictionary<Type, __Entity>();

        public static NpgsqlConnection __conn = null;

        public static DbLocking Locking { get; set; }

        /*public Mapper(NpgsqlConnection conn)
{
__conn = conn;
}*/

        public static T Get<T>(object pk)
        {
            return (T)_CreateObject(typeof(T), pk, null);
        }

        internal static object _CreateObject(Type t, IDataReader re, ICollection<object> localCache)
        {
            __Entity ent = t._GetEntity();
            object rval = _SearchCache(t,
                ent.PrimaryKey.ToFieldType(re.GetValue(re.GetOrdinal(ent.PrimaryKey.ColumnName)), localCache),
                localCache);

            if (rval == null)
            {
                if (localCache == null)
                {
                    localCache = new List<object>();
                }

                localCache.Add(rval = Activator.CreateInstance(t));
            }

            foreach (__Field i in ent.Internals)
            {
                i.SetValue(rval, i.ToFieldType(re.GetValue(re.GetOrdinal(i.ColumnName)), localCache));
            }

            foreach (__Field i in ent.Externals)
            {
                if (typeof(ILazy).IsAssignableFrom(i.Type))
                {
                    i.SetValue(rval, Activator.CreateInstance(i.Type, rval, i.Member.Name));
                }
                else
                {
                    i.SetValue(rval, i.Fill(Activator.CreateInstance(i.Type), rval, localCache));
                }
            }

            return rval;
        }

  
        public static void SaveObject(object obj)
        {
            __Entity ent = obj._GetEntity();
            __Entity bse = obj.GetType().BaseType._GetEntity();

            if (bse.IsMaterial) { SaveObject(obj, bse, false, true); }
            SaveObject(obj, ent, bse.IsMaterial, false);
        }

        private static void SaveObject(object obj, __Entity ent, bool hasMaterialBase, bool isBase)
        {
            if (Cache != null) { if (!Cache.HasChanged(obj)) return; }

            IDbCommand cmd = __conn.CreateCommand();
            string update = "";
            string insert = "";
            cmd.CommandText = ("INSERT INTO " + ent.TableName + " (");
            if (hasMaterialBase)
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
            for (int i = 0; i < ent.LocalInternals.Length; i++)
            {
                if (i > 0) { cmd.CommandText += ", "; insert += ", "; }
                cmd.CommandText += ent.LocalInternals[i].ColumnName;

                insert += (":v" + i.ToString());

                p = cmd.CreateParameter();
                p.ParameterName = (":v" + i.ToString());
                p.Value = ent.LocalInternals[i].ToColumnType(ent.LocalInternals[i].GetValue(obj));
                cmd.Parameters.Add(p);

                if (!ent.LocalInternals[i].IsPrimaryKey)
                {
                    if (first) { first = false; } else { update += ", "; }
                    update += (ent.LocalInternals[i].ColumnName + " = " + (":w" + i.ToString()));

                    p = cmd.CreateParameter();
                    p.ParameterName = (":w" + i.ToString());
                    p.Value = ent.LocalInternals[i].ToColumnType(ent.LocalInternals[i].GetValue(obj));
                    cmd.Parameters.Add(p);
                }
            }
            cmd.CommandText += (") VALUES (" + insert + ") " + update);

            cmd.ExecuteNonQuery();
            cmd.Dispose();

            if (!isBase) foreach (__Field i in ent.Externals) { i.UpdateReferences(obj); }

            if (Cache != null) { Cache.Put(obj); }
        }
        public static void Lock(object obj)
        {
            if (Locking != null) { Locking.Lock(obj); }
        }

        public static ICache Cache { get; set; }

        internal static Type[] _GetChildTypes(this Type t)
        {
            List<Type> rval = new List<Type>();
            foreach (Type i in _Entities.Keys)
            {
                if (t.IsAssignableFrom(i) && (!i.IsAbstract)) { rval.Add(i); }
            }

            return rval.ToArray();
        }

        internal static object _CreateObject(Type t, object pk, ICollection<object> localCache)
        {
            object rval = null;
            int locc = ((localCache != null) ? localCache.Count : 0);

            IDbCommand cmd = __conn.CreateCommand();

            __Entity ent = t._GetEntity();
            cmd.CommandText = ent.GetSQL() + (string.IsNullOrWhiteSpace(ent.SubsetQuery) ? " WHERE " : " AND ") + t._GetEntity().PrimaryKey.ColumnName + " = :pk";

            IDataParameter p = cmd.CreateParameter();
            p.ParameterName = (":pk");
            p.Value = pk;
            cmd.Parameters.Add(p);

            IDataReader re = cmd.ExecuteReader();
            if (re.Read())
            {
                rval = _CreateObject(t, re, localCache);
            }

            re.Close();
            cmd.Dispose();

            if (Cache != null)
            {
                if ((localCache != null) && (localCache.Count > locc)) Cache.Put(rval);
            }

            return rval;
        }
        internal static __Entity _GetEntity(this object o)
        {
            Type t = ((o is Type) ? (Type)o : o.GetType());

            if (!_Entities.ContainsKey(t))
            {
                _Entities.Add(t, new __Entity(t));
            }

            return _Entities[t];
        }



        public static void _GetFieldAttribute(Type t)
        {
            // Get instance of the attribute.
            FieldAttribute MyAttribute =
                (FieldAttribute) Attribute.GetCustomAttribute(t, typeof(FieldAttribute));

            Console.WriteLine(MyAttribute);


        }

        public static void _GetEntityAttribute(Type t)
        {
            // Get instance of the attribute.
            EntityAttribute MyAttribute =
                (EntityAttribute)Attribute.GetCustomAttribute(t, typeof(EntityAttribute));
            Console.WriteLine(MyAttribute.TableName);


        }

        internal static void _FillList(Type t, object list, IDataReader re, ICollection<object> localCache = null)
        {
            while (re.Read())
            {
                list.GetType().GetMethod("Add").Invoke(list, new object[] { _CreateObject(t, re, localCache) });
            }
        }



        /// <summary>Fills a list.</summary>
        /// <param name="t">Type.</param>
        /// <param name="list">List.</param>
        /// <param name="sql">SQL query.</param>
        /// <param name="parameters">Parameters.</param>
        /// <param name="localCache">Local cache.</param>
        internal static void _FillList(Type t, object list, string sql, IEnumerable<Tuple<string, object>> parameters, ICollection<object> localCache = null)
        {
            IDbCommand cmd = __conn.CreateCommand();
            cmd.CommandText = sql;

            foreach (Tuple<string, object> i in parameters)
            {
                IDataParameter p = cmd.CreateParameter();
                p.ParameterName = i.Item1;
                p.Value = i.Item2;
                cmd.Parameters.Add(p);
            }

            IDataReader re = cmd.ExecuteReader();
            _FillList(t, list, re, localCache);
            re.Close();
            re.Dispose();
            cmd.Dispose();
        }


    internal static object _SearchCache(Type t, object pk, ICollection<object> locaclcache)
    {

        if (locaclcache != null)
        {
            foreach (object i in locaclcache)
            {
                if(i.GetType() != t) continue;
                if (i._GetEntity().PrimaryKey.GetValue(i).Equals(pk)) return i;


                }
            } 
        return null;
    }
    }


}
