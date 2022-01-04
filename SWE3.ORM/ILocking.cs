namespace SWE3.ORM
{
    
    public interface ILocking
    {
        
        void Lock(object obj);


        void Release(object obj);
    }
}
