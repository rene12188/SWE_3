using System;
using SWE3.ORM;

namespace SWE3.ExampleApp.School
{
    /// <summary>This is a person implementation (from School example).</summary>
    public abstract class Person
    {
      
        [PrimaryKey]
        public string ID { get; set; }


        /// <summary>Gets or sets the person's name.</summary>
        public string Name { get; set; }


        /// <summary>Gets or sets the person's first name.</summary>
        public string FirstName { get; set; }


        /// <summary>Gets or sets the person's birth date.</summary>
        [Field(ColumnName = "BDATE")]
        public DateTime BirthDate { get; set; }


        /// <summary>Gets or sets the person gender.</summary>
        public int Gender { get; set; }


        
    }


}
