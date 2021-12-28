using System;
using SWE3.ExampleProject.School;
using SWE3.ORM;

namespace SWE3.ExampleProject.Show
{
    /// <summary>This show case demonstrates working with simple foreign keys.</summary>
    public static class WithFK
    {
        /// <summary>Implements the show case.</summary>
        public static void Show()
        {
            Console.WriteLine("(3) Create and load an object with foreign key");
            Console.WriteLine("----------------------------------------------");

            Teacher t = Mapper.Get<Teacher>("t.0");

            Class c = new Class();
            c.ID = "c.0";
            c.Name = "Demonolgy 101";
            c.Teacher = t;

            Mapper.SaveObject(c);

            c = Mapper.Get<Class>("c.0");
            Console.WriteLine((c.Teacher.Salary == 50000) + c.Teacher.Name + " teaches " + c.Name + ".");

            Console.WriteLine("\n");
        }
    }
}
