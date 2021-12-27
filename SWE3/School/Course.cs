using System.Collections.Generic;
using SWE3.ORM.Attributes;

namespace SWE3.ExampleProject.School
{
    /// <summary>This class represents a course in the school model.</summary>
    [Entity(TableName = "COURSES")]
    public class Course
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets or sets the course ID.</summary>
        [PrimaryKey]
        public string ID { get; set; }


        /// <summary>Gets or sets the course name.</summary>
        public string Name { get; set; }


        /// <summary>Gets or sets the course teacher.</summary>
        [SingleForeignKey(ColumnName = "KTEACHER")]
        public Teacher Teacher { get; set; }

        public IList<Student> Students { get; set; }
    }
}
