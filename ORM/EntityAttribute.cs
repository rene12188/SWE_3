﻿using System;



namespace SWE3.Demo.OrmFramework
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
