using System;

namespace SWE3.ORM.Attributes
{
    public class MaterialAttribute: Attribute
    {
        public string TableName;


        public string SubsetQuery;
    }
}
