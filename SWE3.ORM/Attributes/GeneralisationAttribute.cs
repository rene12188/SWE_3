using System;
using System.ComponentModel;

namespace SWE3.ORM
{
    /// <summary>This attribute marks a class as an entity.</summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class GeneralisatinAttribute: Attribute
    {

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public members                                                                                                   //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Table name.</summary>
        public string TableName;
    }
}
