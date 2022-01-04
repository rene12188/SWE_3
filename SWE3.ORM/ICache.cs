﻿using System;

namespace SWE3.ORM
{
    public interface ICache
    {
        object Get(Type t, object pk);


        void Put(object obj);
        

        void Remove(object obj);


        bool Contains(Type t, object pk);


        bool Contains(object obj);


        bool HasChanged(object obj);
    }
}
