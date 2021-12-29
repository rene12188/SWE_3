using System.Collections.Generic;
using SWE3.OrmFramework;


namespace SWE3.ExampleProject.School
{
    /// <summary>This class represents a course in the school model.</summary>
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
