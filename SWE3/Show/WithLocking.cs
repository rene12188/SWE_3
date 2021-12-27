using System;
using SWE3.ExampleProject.School;
using SWE3.ORM;

namespace SWE3.ExampleProject.Show
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

            Mapper.Locking = new DbLocking();
            Teacher t = Mapper.Get<Teacher>("t.0");
            Mapper.Lock(t);

            Mapper.Locking = new DbLocking();

            try
            {
                Mapper.Lock(t);
            }
            catch(Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
