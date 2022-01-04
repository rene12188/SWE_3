using System;
using SWE3.ExampleApp.School;
using SWE3.ORM;

namespace SWE3.ExampleApp.Show
{
    /// <summary>This show case demonstrates working with simple foreign keys.</summary>
    public static class WithFK
    {
        /// <summary>Implements the show case.</summary>
        public static void Show()
        {
            Console.WriteLine("(3) Create and load an object with foreign key");
            Console.WriteLine("----------------------------------------------");

            Teacher t = Orm.Get<Teacher>("t.0");

            Class c = new Class();
            c.ID = "c.0";
            c.Name = "Demonolgy 101";
            c.Teacher = t;

            Orm.Save(c);

            c = Orm.Get<Class>("c.0");
            Console.WriteLine((c.Teacher.Gender == 1 ? "Mr. " : "Ms. ") + c.Teacher.Name + " teaches " + c.Name + ".");

            Console.WriteLine("\n");
        }
    }
}
