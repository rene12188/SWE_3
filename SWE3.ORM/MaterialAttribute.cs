using System;

namespace SWE3.OrmFramework
{
    public class MaterialAttribute: Attribute
    {
        public string TableName;


     
        public string SubsetQuery;
    }
}
