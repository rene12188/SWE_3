using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SWE3.ORM.MetaModel
{
  
    internal class __Entity
    {
      
        private __Field[] _LocalInternals = null;

        
        public __Entity(Type t)
        {
            EntityAttribute tattr = (EntityAttribute) t.GetCustomAttribute(typeof(EntityAttribute));
            if(tattr == null)
            {
                MaterialAttribute mattr = (MaterialAttribute) t.GetCustomAttribute(typeof(MaterialAttribute));
                if(mattr != null)
                {
                    TableName = mattr.TableName;
                    SubsetQuery = mattr.SubsetQuery;
                    IsMaterial = true;
                }
            }
            else 
            { 
                TableName = tattr.TableName;
                SubsetQuery = tattr.SubsetQuery;
                ChildKey = tattr.ChildKey;
            }

            if(string.IsNullOrWhiteSpace(TableName)) { TableName = t.Name.ToUpper(); }

            Member = t;
            List<__Field> fields = new List<__Field>();

            foreach(PropertyInfo i in t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if((IgnoreAttribute) i.GetCustomAttribute(typeof(IgnoreAttribute)) != null) continue;

                __Field field = new __Field(this);

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
                        field.IsExternal = typeof(IEnumerable).IsAssignableFrom(i.PropertyType);

                        field.AssignmentTable  = ((ForeignKeyAttribute) fattr).AssignmentTable;
                        field.RemoteColumnName = ((ForeignKeyAttribute) fattr).RemoteColumnName;
                        field.IsManyToMany = (!string.IsNullOrWhiteSpace(field.AssignmentTable));
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
            Internals = fields.Where(m => (!m.IsExternal)).ToArray();
            Externals  = fields.Where(m => m.IsExternal).ToArray();
        }

        public Type Member
        {
            get; private set;
        }


        public string TableName
        {
            get; private set;
        }


        public string SubsetQuery
        {
            get; private set;
        }
        
        public __Field[] Fields
        {
            get; private set;
        }

        public __Field[] Externals
        {
            get; private set;
        }

        public __Field[] Internals
        {
            get; private set;
        }

        
        public __Field[] LocalInternals
        {
            get
            {
                if(_LocalInternals == null)
                {
                    __Entity bse = Member.BaseType._GetEntity();
                    if(!bse.IsMaterial) { return Internals; }

                    List<__Field> rval =  new List<__Field>();
                    foreach(__Field i in Internals)
                    {
                        if(bse.Internals.Where(m => m.ColumnName == i.ColumnName).Count() == 0) { rval.Add(i); }
                    }

                    _LocalInternals = rval.ToArray();
                }

                return _LocalInternals;
            }
        }


        public __Field PrimaryKey
        {
            get; private set;
        }


        public string ChildKey
        {
            get; private set;
        }


        public bool IsMaterial
        {
            get; private set;
        }



        public string GetSQL()
        {
            __Entity bse = Member.BaseType._GetEntity();

            string rval = "SELECT ";
            for(int i = 0; i < Internals.Length; i++)
            {
                if(i > 0) { rval += ", "; }
                rval += Internals[i].ColumnName;
            }
            rval += (" FROM " + TableName);

            if(bse.IsMaterial)
            {
                rval += " INNER JOIN " + bse.TableName + " ON " + PrimaryKey.ColumnName + " = " + ChildKey;
            }

            if(!string.IsNullOrWhiteSpace(SubsetQuery))
            {
                rval += " WHERE (" + SubsetQuery + ")";
            }

            return rval;
        }


        public __Field GetFieldForColumn(string columnName)
        {
            columnName = columnName.ToUpper();
            foreach(__Field i in Internals)
            {
                if(i.ColumnName.ToUpper() == columnName) { return i; }
            }

            return null;
        }

        public __Field GetFieldByName(string fieldName)
        {
            foreach(__Field i in Fields)
            {
                if(i.Member.Name == fieldName) { return i; }
            }

            return null;
        }
    }
}
