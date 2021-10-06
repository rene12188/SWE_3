using System;
using System.Collections.Generic;
using SWE3.ORM;


namespace SWE3.Demo.SampleApp.School
{
    /// <summary>This is a teacher implementation (from School example).</summary>
    [Entity(TableName = "TEACHERS")]
    public class Teacher: Person
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets or sets the teacher's salary.</summary>
        public int Salary { get; set; }


        [Field(ColumnName = "HDATE")]
        /// <summary>Gets or sets the teacher's hire date.</summary>
        public DateTime HireDate { get; set; }
    }
}
