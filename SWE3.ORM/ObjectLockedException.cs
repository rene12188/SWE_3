using System;

namespace SWE3.ORM
{
    
    public class ObjectLockedException: Exception
    {

        public ObjectLockedException(): base("Object locked by another session.")
        {}
    }
}
