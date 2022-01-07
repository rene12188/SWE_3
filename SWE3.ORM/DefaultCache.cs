using System;
using System.Collections.Generic;

namespace SWE3.ORM
{
    public class DefaultCache: ICache
    {
    
        protected Dictionary<Type, Dictionary<object, object>> _Caches = new Dictionary<Type, Dictionary<object, object>>();

        protected virtual Dictionary<object, object> _GetCache(Type t)
        {
            if(_Caches.ContainsKey(t)) { return _Caches[t]; }

            Dictionary<object, object> rval = new Dictionary<object, object>();
            _Caches.Add(t, rval);

            return rval;
        }


        public virtual object Get(Type t, object pk)
        {
            Dictionary<object, object> c = _GetCache(t);

            if(c.ContainsKey(pk)) { return c[pk]; }
            return null;
        }

        public virtual void Put(object obj)
        {
            if(obj != null) { _GetCache(obj.GetType())[obj._GetEntity().PrimaryKey.GetValue(obj)] = obj; }
        }


        public virtual void Remove(object obj)
        {
            _GetCache(obj.GetType()).Remove(obj._GetEntity().PrimaryKey.GetValue(obj));
        }

        public virtual bool Contains(Type t, object pk)
        {
            return _GetCache(t).ContainsKey(pk);
        }

        public virtual bool Contains(object obj)
        {
            return _GetCache(obj.GetType()).ContainsKey(obj._GetEntity().PrimaryKey.GetValue(obj));
        }


        public virtual bool HasChanged(object obj)
        {
            return true;
        }
    }
}
