namespace SWE3.OrmFramework
{
   
    public class ForeignKeyAttribute: FieldAttribute
    {
      
        public string AssignmentTable = null;

      
        public string RemoteColumnName = null;
    }
}
