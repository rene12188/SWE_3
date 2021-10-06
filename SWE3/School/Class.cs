using System;
using SWE3.ORM;


namespace SWE3.Demo.SampleApp.School
{
    /// <summary>This class represents a class in the school model.</summary>
    [Entity(TableName = "CLASSES")]
    public class Class
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets or sets the class ID.</summary>
        [PrimaryKey]
        public string ID { get; set; }


        /// <summary>Gets or sets the class name.</summary>
        public string Name { get; set; }


        /// <summary>Gets or sets the class teacher.</summary>
        [ForeignKey(ColumnName = "KTEACHER")]
        public Teacher Teacher
        {
            get; set;
        }
    }
}
