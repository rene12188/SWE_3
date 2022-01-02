using System;

namespace SWE3.ORM
{
    /// <summary>This attribute marks a class as an entity.</summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityAttribute: Attribute
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public members                                                                                                   //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Table name.</summary>
        public string TableName;


        /// <summary>Provides a WHERE-clause that defines a subset of the entity table.</summary>
        public string SubsetQuery;


        /// <summary>Foreign key that references master table.</summary>
        public string ChildKey;
    }
}
