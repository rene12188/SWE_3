using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using SWE3.ORM.MetaModel;

namespace SWE3.ORM
{
    public class Mapper
    {
        internal NpgsqlConnection __conn = null;
        public Mapper(NpgsqlConnection conn)
        {
            __conn = conn;
        }
        public bool SaveObject(object obj)
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
                Console.WriteLine(InsertTextBuilder(ent, obj));
            }

            Console.WriteLine(types);
            return false;
            GetEntityAttribute(type);


            return false;
        }

        public static void GetFieldAttribute(Type t)
        {
            // Get instance of the attribute.
            FieldAttribute MyAttribute =
                (FieldAttribute) Attribute.GetCustomAttribute(t, typeof(FieldAttribute));

            Console.WriteLine(MyAttribute);


        }

        public static void GetEntityAttribute(Type t)
        {
            // Get instance of the attribute.
            EntityAttribute MyAttribute =
                (EntityAttribute)Attribute.GetCustomAttribute(t, typeof(EntityAttribute));
            Console.WriteLine(MyAttribute.TableName);


        }

        internal  string InsertTextBuilder(__Entity ent, object obj)
        {
            int fieldnumber = ent.Fields.Length;

            string fields = "(";

            string values = "(";

            for (int i = 0; i < ent.Fields.Length; i++)
            {
                fields += ent.Fields[i].ColumnName.ToString();

                values += "'" +ent.Fields[i].ToColumnType(ent.Fields[i].GetValue(obj)) +"'";

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
            return cmdtxt;
        }


    }


}
