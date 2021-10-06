using System;
using SWE3.ORM;


namespace SWE3.Demo.SampleApp.School
{
    /// <summary>This is a student implementation (from School example).</summary>
    [Entity(TableName = "STUDENTS")]
    public class Student: Person
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets or sets the student's grade.</summary>
        public int Grade { get; set; }
    }
}
