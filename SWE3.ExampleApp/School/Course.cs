using System.Collections.Generic;
using SWE3.ORM;
using SWE3.ORM.Attributes;

namespace SWE3.ExampleApp.School
{
    [Entity(TableName = "COURSES")]
    public class Course
    {
       
        [PrimaryKey]
        public string ID { get; set; }


        public string Name { get; set; }


        [ForeignKey(ColumnName = "KTEACHER")]
        public Teacher Teacher { get; set; }


        [ForeignKey(AssignmentTable = "STUDENT_COURSES", ColumnName = "KCOURSE", RemoteColumnName = "KSTUDENT")]
        public List<Student> Students { get; set; } = new List<Student>();
    }
}
