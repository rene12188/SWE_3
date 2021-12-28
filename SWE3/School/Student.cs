﻿using SWE3.ORM.Attributes;

namespace SWE3.ExampleProject.School
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


        /// <summary>Gets the student's class.</summary>
        [ForeignKey(ColumnName = "KCLASS")]
        public Class Class { get; set; }
    }
}
