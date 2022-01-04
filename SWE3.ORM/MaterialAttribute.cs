using System;

namespace SWE3.ORM
{
    public class MaterialAttribute: Attribute
    {
        public string TableName;


        public string SubsetQuery;
    }
}
