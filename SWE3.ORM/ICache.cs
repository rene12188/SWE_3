using System;

namespace SWE3.OrmFramework
{
    public interface ICache
    {
   
        object Read(Type t, object pk);


     
        void Create(object obj);
        

        void Delete(object obj);


   
        bool Find(Type t, object pk);


       
        bool Find(object obj);


      
        bool HasChanged(object obj);
    }
}
