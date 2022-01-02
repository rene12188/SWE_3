namespace SWE3.ORM
{
    /// <summary>Locking mechanism providers implement this interface.</summary>
    public interface ILocking
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // methods                                                                                                          //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Locks an object.</summary>
        /// <param name="obj">Object.</param>
        /// <exception cref="ObjectLockedException">Thrown when the object is already locked by another instance.</exception>
        void Lock(object obj);


        /// <summary>Releases a lock on an object.</summary>
        /// <param name="obj">Object.</param>
        void Release(object obj);
    }
}
