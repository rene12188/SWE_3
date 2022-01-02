using System.Threading;
using Npgsql;
using SWE3.ExampleProject.Show;
using SWE3.OrmFramework;

namespace SWE3.ExampleProject
{
    public class Demo
    {
        public static void Main()
        {
            Mapper.Connectionstring = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=a;Database=swe3;";

            Mapper.Cache = new BasicCache();

            InsertObject.Show();
            Thread.Sleep(2000);
            ModifyObject.Show();
            Thread.Sleep(2000);

            WithFK.Show();
            Thread.Sleep(2000);

            WithMToN.Show();

            Thread.Sleep(2000);
            WithLazyList.Show();
            Thread.Sleep(2000);
            WithCache.Show();
            Thread.Sleep(2000);

            WithLocking.Show();

        }
        
    }
}