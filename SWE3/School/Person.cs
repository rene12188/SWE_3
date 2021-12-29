using System;
using SWE3.OrmFramework;

namespace SWE3.ExampleProject.School
{
    public abstract class Person
    {
       
        protected static int _N = 1;



    
        [PrimaryKey]
        public string ID { get; set; }


        public string Name { get; set; }


        public string FirstName { get; set; }


        [Field(ColumnName = "BDATE")]
        public DateTime BirthDate { get; set; }


        public bool IsMale { get; set; }


        [Ignore]
        public int InstanceNumber { get; protected set; } = _N++;
    }


}
