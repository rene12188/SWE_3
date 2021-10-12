using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace SWE3.ORM.MetaModel
{
    /// <summary>This class holds entity metadata.</summary>
    internal class __Entity
    {


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Creates a new instance of this class.</summary>
        /// <param name="t">Type.</param>
        public __Entity(Type t , __Entity parent)
        {
            EntityAttribute tattr = (EntityAttribute) t.GetCustomAttribute(typeof(EntityAttribute));
            if((tattr == null) || (string.IsNullOrWhiteSpace(tattr.TableName)))
            {
                TableName = t.Name.ToUpper();
            }
            else { TableName = tattr.TableName; }

            Member = t;
            List<__Field> fields = new List<__Field>();

            foreach(PropertyInfo i in t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly
                | System.Reflection.BindingFlags.DeclaredOnly))
            {
                if((IgnoreAttribute) i.GetCustomAttribute(typeof(IgnoreAttribute)) != null) continue;
                if ((ForeignKeyAttribute)i.GetCustomAttribute(typeof(ForeignKeyAttribute)) != null) continue;

                __Field field = new __Field(this);


                if ((ChildAttribute) i.GetCustomAttribute(typeof(ChildAttribute)) != null)
                {
                    field.GiveToChild = true;
                }


                FieldAttribute fattr = (FieldAttribute) i.GetCustomAttribute(typeof(FieldAttribute));

                if(fattr != null)
                {
                    if(fattr is PrimaryKeyAttribute)
                    {
                        PrimaryKey = field;
                        field.IsPrimaryKey = true;
                    }

                    field.ColumnName = (fattr?.ColumnName ?? i.Name);
                    field.ColumnType = (fattr?.ColumnType ?? i.PropertyType);

                    field.IsNullable = fattr.Nullable;

                    field.IsForeignKey = (fattr is ForeignKeyAttribute);
                }
                else
                {
                    if((i.GetGetMethod() == null) || (!i.GetGetMethod().IsPublic)) continue;

                    field.ColumnName = i.Name;
                    field.ColumnType = i.PropertyType;
                }

                field.Member = i;

                fields.Add(field);
            }

            if (parent == null)
            {
                Fields = fields.ToArray();
            }
            else
            {
                bool generalised = Mapper.GetGeneralisationAttribute(parent.Member);
                foreach (__Field var in parent.Fields)
                { 

                    if (var.GiveToChild || generalised)
                    {
                        fields.Add(var);
                        
                    }
                    else
                    {
                        continue;
                    }


                }

              
            }
            Fields = fields.ToArray();

            Internals = fields.Where(m => (!m.IsExternal)).ToArray();
            Internals = fields.Where(m => (!m.IsInternal)).ToArray();

        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets the member type.</summary>
        public Type Member
        {
            get; private set;
        }


        /// <summary>Gets the table name.</summary>
        public string TableName
        {
            get; private set;
        }


        /// <summary>Gets the entity fields.</summary>
        public __Field[] Fields
        {
            get; private set;
        }

        public __Field[] Internals
        {
            get; private set;
        }

        public __Field[] Externals
        {
            get; private set;
        }

        /// <summary>Gets the entity primary key.</summary>
        public __Field PrimaryKey
        {
            get; private set;
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public methods                                                                                                   //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets the entity SQL.</summary>
        /// <param name="prefix">Prefix.</param>
        /// <returns>SQL string.</returns>
        public string GetSQL(string prefix = null)
        {
            if(prefix == null)
            {
                prefix = "";
            }

            string rval = "SELECT ";
            for(int i = 0; i < Fields.Length; i++)
            {
                if(i > 0) { rval += ", "; }
                rval += prefix.Trim() + Fields[i].ColumnName;
            }
            rval += (" FROM " + TableName);

            return rval;
        }
    }
}
