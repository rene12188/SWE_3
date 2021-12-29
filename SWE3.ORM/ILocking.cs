namespace SWE3.OrmFramework
{
    public interface ILocking
    {
        
        void Lock(object obj);


        
        void Release(object obj);
    }
}
