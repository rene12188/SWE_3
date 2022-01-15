using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE3.ExampleApp.School;
using SWE3.ORM;

namespace SWE.Tests
{
    [TestFixture]
    public class CacheTests
    {
      
       
        [Test]
        public void Cache_Put_NoError()
        {
         Cache cacheObject = new Cache();
        Teacher t = new Teacher();
            t.ID = "T.1";
            cacheObject.Put(t);
            Teacher tmp = (Teacher)cacheObject.Get(typeof(Teacher), t.ID);
            Assert.AreEqual(t.ID, tmp.ID);
        }

        [Test]
        public void Cache_Contains_True()
        {
            Cache cacheObject = new Cache();
            Teacher t = new Teacher();
            t.ID = "T.1";
            cacheObject.Put(t);
          
            Assert.IsTrue(cacheObject.Contains(t));
        }

        [Test]
        public void Cache_Contains_False()
        {
            Cache cacheObject = new Cache();
            Teacher t = new Teacher();
            t.ID = "T.1";


            Assert.IsFalse(cacheObject.Contains(t));
        }


        [Test]
        public void Cache_HasChanged_True()
        {
            Cache cacheObject = new Cache();
            Teacher t = new Teacher();
            t.ID = "T.1";
            t.Salary = 1000;
            cacheObject.Put(t);
            t.Salary = 2000;
            Assert.IsFalse(cacheObject.HasChanged(t));
        }

        [Test]
        public void Cache_HasChanged_False()
        {
            Cache cacheObject = new Cache();
            Teacher t = new Teacher();
            t.ID = "T.1";
            t.Salary = 1000;
            cacheObject.Put(t);
            Assert.IsTrue(cacheObject.HasChanged(t));
        }

        [Test]
        public void Cache_Remove_NoError()
        {
            Cache cacheObject = new Cache();
            Teacher t = new Teacher();
            t.ID = "T.1";
            cacheObject.Put(t);
            cacheObject.Remove(t);
            Assert.IsFalse(cacheObject.Contains(t));
        }
     
    }
}
