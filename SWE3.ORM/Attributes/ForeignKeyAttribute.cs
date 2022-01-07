namespace SWE3.ORM.Attributes
{
    public class ForeignKeyAttribute: FieldAttribute
    {
      
        public string AssignmentTable = null;

        public string RemoteColumnName = null;
    }
}
