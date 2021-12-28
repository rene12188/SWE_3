using System;

namespace SWE3.ORM.Attributes
{
    /// <summary>This attribute marks a class as a material base entity.</summary>
    public class MaterialAttribute: Attribute
    {
        /// <summary>Table name.</summary>
        public string TableName;


        /// <summary>Provides a WHERE-clause that defines a subset of the entity table.</summary>
        public string SubsetQuery;
    }
}
