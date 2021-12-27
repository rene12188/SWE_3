using System;
using SWE3.ExampleProject.School;

namespace SWE3.ExampleProject.Show
{
    public class WithFKList
    {
        /// <summary>This show case loads a teacher with a list of classes.</summary>
        public static void Show()
        {
            Console.WriteLine("(4) Load teacher and show classes");
            Console.WriteLine("---------------------------------");

            Teacher t = MarshalByRefObject.Get<Teacher>("t.0");

            Console.WriteLine(t.FirstName + " " + t.Name + " teaches:");

            foreach(Class i in t.Classes)
            {
                Console.WriteLine(i.Name);
            }

            Console.WriteLine("\n");
        }
    }
}
