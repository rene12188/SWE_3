using System;
using System.Collections.Generic;
using System.Reflection;



namespace SWE3.Demo.OrmFramework.MetaModel
{
    /// <summary>This class holds entity metadata.</summary>
    internal class MEntity
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Creates a new instance of this class.</summary>
        /// <param name="t">Type.</param>
        public MEntity(Type t)
        {
            EntityAttribute tattr = (EntityAttribute) t.GetCustomAttribute(typeof(EntityAttribute));
            if((tattr == null) || (string.IsNullOrWhiteSpace(tattr.TableName)))
            {
                TableName = t.Name.ToUpper();
            }
            else { TableName = tattr.TableName; }

            Member = t;
            List<MField> fields = new List<MField>();

            foreach(PropertyInfo i in t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if((IgnoreAttribute) i.GetCustomAttribute(typeof(IgnoreAttribute)) != null) continue;

                MField field = new MField(this);

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

                    if(field.IsForeignKey = (fattr is ForeignKeyAttribute))
                    {
                        field.IsForeignKey = true;
                    }
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

            Fields = fields.ToArray();
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
        public MField[] Fields
        {
            get; private set;
        }


        /// <summary>Gets the entity primary key.</summary>
        public MField PrimaryKey
        {
            get; private set;
        }
    }
}
