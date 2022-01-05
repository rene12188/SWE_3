using System;
using SWE3.ExampleApp.School;
using SWE3.ORM;

namespace SWE3.ExampleApp.Show
{
    /// <summary>This show case demonstrates cache functionality.</summary>
    public static class WithCache
    {
        /// <summary>Implements the show case.</summary>
        public static void Show()
        {
            Console.WriteLine("(6) Cache demonstration");
            Console.WriteLine("-----------------------");

            Console.WriteLine("\nWithout cache:");
            _ShowInstances();

            Console.WriteLine("\nWith cache:");
            Orm.Cache = new TrackingCache();
            _ShowInstances();
        }


        /// <summary>Shows instances.</summary>
        private static void _ShowInstances()
        {
            for(int i = 0; i < 7; i++)
            {
                Teacher t = Orm.Get<Teacher>("t.0");
              
            }
        }
    }
}
