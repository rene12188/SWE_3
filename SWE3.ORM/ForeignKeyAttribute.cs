namespace SWE3.ORM
{
    public class ForeignKeyAttribute: FieldAttribute
    {
      
        public string AssignmentTable = null;

        public string RemoteColumnName = null;
    }
}
