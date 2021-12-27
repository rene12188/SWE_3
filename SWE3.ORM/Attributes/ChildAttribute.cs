using System;

namespace SWE3.ORM.Attributes
{
    /// <summary>This attribute marks a member as a field.</summary>
    public class ChildAttribute: Attribute
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public members                                                                                                   //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Database column name.</summary>
        public string ColumnName = null;

        /// <summary>Database column type.</summary>
        public Type ColumnType = null;

        /// <summary>Nullable flag.</summary>
        public bool Nullable = false;
    }
}
