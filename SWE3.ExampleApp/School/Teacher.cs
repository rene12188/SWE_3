using System;
using System.Collections.Generic;
using SWE3.ORM;
using SWE3.ORM.Attributes;

namespace SWE3.ExampleApp.School
{
    [Entity(TableName = "TEACHERS")]
    public class Teacher: Person
    {
        public int Salary { get; set; }


        [Field(ColumnName = "HDATE")]
        public DateTime HireDate { get; set; }


        [ForeignKey(ColumnName = "KTEACHER")]
        public List<Class> Classes { get; private set; } = new List<Class>();
    }
}
