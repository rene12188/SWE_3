using System;
using SWE3.ORM;
using SWE3.ORM.Attributes;

namespace SWE3.ExampleApp.School
{
    public abstract class Person
    {
      
        [PrimaryKey]
        public string ID { get; set; }


        public string Name { get; set; }


        public string FirstName { get; set; }


        [Field(ColumnName = "BDATE")]
        public DateTime BirthDate { get; set; }

        
        public int Gender { get; set; }


        
    }


}
