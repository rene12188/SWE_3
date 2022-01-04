using System;
using SWE3.ExampleApp.School;
using SWE3.ORM;

namespace SWE3.ExampleApp.Show
{
    /// <summary>This show case demonstrates locking.</summary>
    public static class WithLocking
    {
        /// <summary>Implements the show case.</summary>
        public static void Show()
        {
            Console.WriteLine("(8) Locking demonstration");
            Console.WriteLine("-------------------------");
            Console.WriteLine();

            Orm.Locking = new DbLocking();
            Teacher t = Orm.Get<Teacher>("t.0");
            Orm.Lock(t);

            Orm.Locking = new DbLocking();

            try
            {
                Orm.Lock(t);
            }
            catch(Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
