using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using SWE3.ORM.MetaModel;

namespace SWE3.ORM
{
    public class Cache:  ICache
    {
       
        protected Dictionary<Type, Dictionary<object, string>> _Hashes = new Dictionary<Type, Dictionary<object, string>>();

        protected Dictionary<Type, Dictionary<object, object>> _Caches = new Dictionary<Type, Dictionary<object, object>>();

        protected virtual Dictionary<object, object> _GetCache(Type t)
        {
            if (_Caches.ContainsKey(t)) { return _Caches[t]; }

            Dictionary<object, object> rval = new Dictionary<object, object>();
            _Caches.Add(t, rval);

            return rval;
        }


        public virtual object Get(Type t, object pk)
        {
            Dictionary<object, object> c = _GetCache(t);

            if (c.ContainsKey(pk)) { return c[pk]; }
            return null;
        }



        public virtual bool Contains(Type t, object pk)
        {
            return _GetCache(t).ContainsKey(pk);
        }

        public virtual bool Contains(object obj)
        {
            return _GetCache(obj.GetType()).ContainsKey(obj._GetEntity().PrimaryKey.GetValue(obj));
        }



        protected virtual Dictionary<object, string> _GetHash(Type t)
        {
            if(_Hashes.ContainsKey(t)) { return _Hashes[t]; }

            Dictionary<object, string> rval = new Dictionary<object, string>();
            _Hashes.Add(t, rval);

            return rval;
        }

        protected string _ComputeHash(object obj)
        {
            string rval = "";
            foreach(__Field i in obj._GetEntity().Internals) 
            {
                object m = i.GetValue(obj);

                if(m != null)
                {
                    if(i.IsForeignKey)
                    {
                        if(m != null) { rval += m._GetEntity().PrimaryKey.GetValue(m).ToString(); }
                    }
                    else { rval += (i.ColumnName + "=" + m.ToString() + ";"); }
                }
            }

            foreach(__Field i in obj._GetEntity().Externals)
            {
                IEnumerable m = (IEnumerable) i.GetValue(obj);

                if(m != null)
                {
                    rval += (i.ColumnName + "=");
                    foreach(object k in m)
                    {
                        rval += k._GetEntity().PrimaryKey.GetValue(k).ToString() + ",";
                    }
                }
            }

            return Encoding.UTF8.GetString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(rval)));
        }


        public  void Put(object obj)
        {
            if (obj != null) { _GetCache(obj.GetType())[obj._GetEntity().PrimaryKey.GetValue(obj)] = obj; }
            if (obj != null) { _GetHash(obj.GetType())[obj._GetEntity().PrimaryKey.GetValue(obj)] = _ComputeHash(obj); }
        }


        public  void Remove(object obj)
        {

            _GetCache(obj.GetType()).Remove(obj._GetEntity().PrimaryKey.GetValue(obj));
            _GetHash(obj.GetType()).Remove(obj._GetEntity().PrimaryKey.GetValue(obj));
        }

        public bool HasChanged(object obj)
        {
            Dictionary<object, string> h = _GetHash(obj.GetType());
            object pk = obj._GetEntity().PrimaryKey.GetValue(obj);

            if(h.ContainsKey(pk))
            {
                return (h[pk] == _ComputeHash(obj));
            }

            return true;
        }
    }
}
