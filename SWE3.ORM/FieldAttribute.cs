using System;

namespace SWE3.ORM
{
    public class FieldAttribute: Attribute
    {
       
        public string ColumnName = null;

        public Type ColumnType = null;

        public bool Nullable = false;
    }
}
