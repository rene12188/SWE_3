using System;
using SWE3;
using SWE3.ORM;


namespace SWE3.Demo.SampleApp
{
    /// <summary>This is a person implementation (from School example).</summary>
    [GeneralisatinAttribute]
    public abstract class Person
    {

        protected static int _N = 1;

        [ChildAttribute]
        [PrimaryKey]
        public string ID { get; set; }

        public string Name { get; set; }

        public string FirstName { get; set; }

        [Field(ColumnName = "BDATE")]
        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        [Ignore]
        public int InstanceNumber { get; protected set; } = _N++;
    }

    public enum Gender: int
    {
        FEMALE = 0,
        MALE = 1
    }
}
