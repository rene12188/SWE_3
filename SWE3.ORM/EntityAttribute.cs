using System;

namespace SWE3.OrmFramework
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityAttribute: Attribute
    {
      
        public string TableName;


     
        public string SubsetQuery;


      
        public string ChildKey;
    }
}
