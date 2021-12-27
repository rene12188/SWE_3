using System;

namespace SWE3.ORM.Attributes
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
    }
}
