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

        internal static NpgsqlConnection __conn = null;
        /*public Mapper(NpgsqlConnection conn)
        {
            __conn = conn;
        }*/
        public static bool SaveObject(object obj)
        {
            List<Type> types = new List<Type>();
            
            __Entity parent = null;
            Type type = obj.GetType();
            while (type.Name != "Object")
            {
                types.Add(type);
                type = type.BaseType;



            }

            types.Reverse();



            foreach (var tp in types)
            {
                var ent = new __Entity(tp, parent);

                parent = ent;

                if(!_GetGeneralisationAttribute(parent.Member))
                    InsertTextBuilder(ent, obj);
            }

            Console.WriteLine(types);
            return false;

        }

      /*  public static  T Get<T>(string ID)
        {

            return new T();
        }*/


        public static object _CreateObject(Type t, IDataReader re, ICollection<object> localcache)
        {

            __Entity ent = t._GetEntity();
            object rval = _SearchCache(t, re.GetValue(re.GetOrdinal(ent.PrimaryKey.ColumnName)), localcache);

            if (rval == null)
            {
                if (localcache == null)
                {
                    localcache = new List<object>();
                }
                localcache.Add(Activator.CreateInstance(t));
            }

            return rval;

            //old
            /*
            object rval = Activator.CreateInstance(t);
        
            foreach (__Field i in t._GetEntity().Fields)
            {
                i.SetValue(rval, i.ToFieldType(re.GetValue(re.GetOrdinal(i.ColumnName))));
            }

            return rval;*/
        }
        internal static __Entity _GetEntity(this object o)
        {
            Type t = ((o is Type) ? (Type)o : o.GetType());

            if (!_Entities.ContainsKey(t))
            {
                _Entities.Add(t, new __Entity(t, null));
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

        public static bool _GetGeneralisationAttribute(Type t)
        {
            // Get instance of the attribute.
            GeneralisatinAttribute MyAttribute =
                (GeneralisatinAttribute)Attribute.GetCustomAttribute(t, typeof(GeneralisatinAttribute));
            if (MyAttribute != null)
                return true;


            return false;
        }

      

    


    internal static string InsertTextBuilder(__Entity ent, object obj)
    {   
            int singlepropcount = 0;
            int fieldnumber = ent.Fields.Length;

            string fields = "(";

            string values = "(";

            for (int i = 0; i < ent.Fields.Length; i++)
            {
                fields += ent.Fields[i].ColumnName.ToString();

                if (ent.Fields[i].IsSingleForeignKey)
                {

                   var tmp =  ent.Fields[i].ToColumnType(ent.Fields[i].GetValue(obj));
                   SaveObject(tmp);
                   values += "'" + tmp.ToString() + "'";
                }
                else
                {
                    values += "'" + ent.Fields[i].ToColumnType(ent.Fields[i].GetValue(obj)) + "'";
                }
               

                if (i == ent.Fields.Length - 1)
                {
                    break;
                }
                fields += ",";
                values += ",";
            }

            fields += ")";
            values += ")";


            string cmdtxt = $"INSERT INTO {ent.TableName} {fields} Values {values} ;";
            Console.WriteLine(cmdtxt);
            return cmdtxt;
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
