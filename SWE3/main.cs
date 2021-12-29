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
            Mapper.Connection = new NpgsqlConnection("Server=127.0.0.1;Port=5432;User Id=postgres;Password=a;Database=swe3;");
            Mapper.Connection.Open();
            InsertObject.Show();
            Thread.Sleep(10);
            ModifyObject.Show();
            Thread.Sleep(10);
            WithCache.Show();


        }
        
    }
}