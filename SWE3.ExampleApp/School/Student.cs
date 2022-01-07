using SWE3.ORM;
using SWE3.ORM.Attributes;

namespace SWE3.ExampleApp.School
{
    [Entity(TableName = "STUDENTS")]
    public class Student: Person
    {
        
        public int Grade { get; set; }


        [ForeignKey(ColumnName = "KCLASS")]
        public Class Class { get; set; }
    }
}
