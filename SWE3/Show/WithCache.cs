using System;
using SWE3.ExampleProject.School;
using SWE3.OrmFramework;

namespace SWE3.ExampleProject.Show
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
            Mapper.Cache = new DefaultCache();
            _ShowInstances();
        }


        /// <summary>Shows instances.</summary>
        private static void _ShowInstances()
        {
            for(int i = 0; i < 7; i++)
            {
                Teacher t = Mapper.Get<Teacher>("t.0");
                Console.WriteLine("Object [" + t.ID + "] instance no: " + t.InstanceNumber.ToString());
            }
        }
    }
}
