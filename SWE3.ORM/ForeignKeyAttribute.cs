namespace SWE3.OrmFramework
{
   //Use this Attribute to mark a Foreignkey
    public class ForeignKeyAttribute: FieldAttribute
    {
      
        public string AssignmentTable = null;

      
        public string RemoteColumnName = null;
    }
}
