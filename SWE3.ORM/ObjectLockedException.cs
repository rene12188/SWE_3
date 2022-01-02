using System;

namespace SWE3.ORM
{
    /// <summary>This exception is thrown when trying to lock an object that has already been locked by another instance.</summary>
    public class ObjectLockedException: Exception
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Creates a new instance of this class.</summary>
        public ObjectLockedException(): base("Object locked by another session.")
        {}
    }
}
