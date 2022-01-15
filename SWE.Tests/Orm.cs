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
        }

        [Test]
        public void Test1()
        {
          
        }
    }
}