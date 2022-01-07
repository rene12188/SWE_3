using System;

namespace SWE3.ORM.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityAttribute: Attribute
    {
        public string TableName;


        public string SubsetQuery;


        public string ChildKey;
    }
}
