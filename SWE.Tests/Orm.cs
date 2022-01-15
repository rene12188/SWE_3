using System;
using System.Collections.Generic;
using NUnit.Framework;
using SWE3.ExampleApp.School;
using SWE3.ORM;

namespace SWE.Tests
{
    public class Tests
    {
        
        [SetUp]
        public void Setup()
        {
            Orm.Connectionstring = "Data Source=test.sqlite;Version=3";
            Orm.Target = Orm.DBTYPE.SQLite;
            Orm.Cache = new Cache();
        }
        [Order(1)]
        [Test]
        public void ORM_Save_Error()
        {
            Teacher t = new Teacher();

           

            Assert.Throws<KeyNotFoundException>(() =>Orm.Get<Teacher>("t.0"));

            

        }
        [Order(2)]
        [Test]
        public void ORM_Save_NoError()
        {
            Teacher t = new Teacher();

            t.ID = "t.0";
            t.FirstName = "Jerry";
            t.Name = "Mouse";
            t.Gender = 1;
            t.BirthDate = new DateTime(1970, 8, 18);
            t.HireDate = new DateTime(2015, 6, 20);
            t.Salary = 50000;

            Orm.Save(t);

            Assert.IsInstanceOf<Teacher>(Orm.Get<Teacher>("t.0"));



        }
               [Order(3)]
        [Test]
        public void ORM_Delete_NoError()
        {
            Teacher t = new Teacher();

            t.ID = "t.0";
            t.FirstName = "Jerry";
            t.Name = "Mouse";
            t.Gender = 1;
            t.BirthDate = new DateTime(1970, 8, 18);
            t.HireDate = new DateTime(2015, 6, 20);
            t.Salary = 50000;

            Orm.Delete(t);



            Assert.Throws<KeyNotFoundException>(() => Orm.Get<Teacher>("t.0"));



        }



    }
}