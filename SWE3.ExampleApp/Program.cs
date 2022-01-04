using System.Data.SQLite;
using SWE3.ExampleApp.Show;
using SWE3.ORM;

namespace SWE3.ExampleApp
{
    /// <summary>This is the main program class for this sample application.</summary>
    class Program
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // entry point                                                                                                      //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>This is the program main entry point.</summary>
        /// <param name="args">Command line arguments.</param>
        static void Main(string[] args)
        {
            //Orm.Connection = new SQLiteConnection("Data Source=test.sqlite;Version=3;");
            Orm.Connectionstring = "Server=localhost:5432 ;Database=swe3 ;User Id=postgres;Password=a;";

            Orm.Cache = new TrackingCache();

            InsertObject.Show(); 
            ModifyObject.Show();
            //WithFK.Show();
            WithFKList.Show();
            WithMToN.Show();
           // WithLazyList.Show();
         //   WithCache.Show();
          //  WithQuery.Show();
          //  WithLocking.Show();

            Orm.Connection.Close();
        }
    }
}
