using Npgsql;
using SWE3.ExampleProject.Show;
using SWE3.ORM;

namespace SWE3.ExampleProject
{
    public class Demo
    {
        public static void Main()
        {
            Mapper.__conn = new NpgsqlConnection("Server=127.0.0.1;Port=5432;User Id=postgres;Password=a;Database=swe3;");
            Mapper.__conn.Open();
            InsertObject.Show();


        }
        
    }
}