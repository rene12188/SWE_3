using System;
using System.Collections.Generic;

namespace SWE3.OrmFramework
{
 
    public class BasicCache: ICache
    {
        protected Dictionary<Type, Dictionary<object, object>> _InternalCaches = new Dictionary<Type, Dictionary<object, object>>();



  
        protected virtual Dictionary<object, object> _GetCache(Type t)
        {
            if (_InternalCaches.ContainsKey(t))
            {
                return _InternalCaches[t];
            }
            else
            {
                Dictionary<object, object> rval = new Dictionary<object, object>();
                _InternalCaches.Add(t, rval);

                return rval;
            }

         
        }


        public virtual object Read(Type t, object pk)
        {
            Dictionary<object, object> c = _GetCache(t);

            if(c.ContainsKey(pk)) { return c[pk]; }
            return null;
        }


 
        public virtual void Create(object obj)
        {
            if(obj != null) { _GetCache(obj.GetType())[obj._GetEntity().PrimaryKey.GetValue(obj)] = obj; }
        }


        public virtual void Delete(object obj)
        {
            _GetCache(obj.GetType()).Remove(obj._GetEntity().PrimaryKey.GetValue(obj));
        }


        public virtual bool Find(Type t, object pk)
        {
            return _GetCache(t).ContainsKey(pk);
        }

        public virtual bool Find(object obj)
        {
            return _GetCache(obj.GetType()).ContainsKey(obj._GetEntity().PrimaryKey.GetValue(obj));
        }

        public virtual bool HasChanged(object obj)
        {
            return true;
        }
    }
}
