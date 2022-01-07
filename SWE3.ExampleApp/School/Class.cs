

using SWE3.ORM;
using SWE3.ORM.Attributes;

namespace SWE3.ExampleApp.School
{
    [Entity(TableName = "CLASSES")]
    public class Class
    {
        public Class()
        {
            Students = new LazyList<Student>(this, "Students");
        }


        [ForeignKey(ColumnName = "KTEACHER")]
        private LazyObject<Teacher> _Teacher { get; set; } = new LazyObject<Teacher>();


        [PrimaryKey]
        public string ID { get; set; }


        public string Name { get; set; }


        [Ignore]
        public Teacher Teacher
        {
            get { return _Teacher.Value; }
            set { _Teacher.Value = value; }
        }

        [ForeignKey(ColumnName = "KCLASS")]
        public LazyList<Student> Students
        {
            get; set;
        }
    }
}
