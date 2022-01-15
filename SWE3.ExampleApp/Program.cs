using System.Data.SQLite;
using SWE3.ExampleApp.Show;
using SWE3.ORM;

namespace SWE3.ExampleApp
{
    class Program
    {

        static void Main(string[] args)
        {
            



            //Orm.Connectionstring = "Server=localhost:5432 ;Database=swe3 ;User Id=postgres;Password=a;";
            //Orm.Target = Orm.DBTYPE.Postgres;



            Orm.Connectionstring = "Data Source=test.sqlite;Version=3";
            Orm.Target = Orm.DBTYPE.SQLite;
            Orm.Cache = new Cache();

            InsertObject.Show(); 
            ModifyObject.Show();
            WithFK.Show();
            WithFKList.Show();
            WithMToN.Show();
            WithLazyList.Show();
         
            WithCache.Show();
            WithQuery.Show();
            WithLocking.Show();
       
        }
    }
}
