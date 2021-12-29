using System;

namespace SWE3.OrmFramework
{
 
    public class ObjectLockedException: Exception
    {
       
        public ObjectLockedException(): base("Object locked by another session.")
        {}
    }
}
