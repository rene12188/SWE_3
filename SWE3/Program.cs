using System.Data.SQLite;
using SWE3.ExampleProject.Show;
using SWE3.ORM;

namespace SWE3.ExampleProject
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
            Mapper.Connection = new SQLiteConnection("Data Source=DB.sqlite;Version=3;");
            Mapper.Connection.Open();

            Mapper.Cache = new TrackingCache();

            InsertObject.Show();
            ModifyObject.Show();
            WithFK.Show();
            WithFKList.Show();
            WithMToN.Show();
            WithLazyList.Show();
            WithCache.Show();
            WithQuery.Show();
            WithLocking.Show();

            Mapper.Connection.Close();
        }
    }
}
