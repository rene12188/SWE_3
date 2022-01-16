using System;
using SWE3.ExampleApp.School;
using SWE3.ORM;

namespace SWE3.ExampleApp.Show
{
    /// <summary>This show case uses lazy loading.</summary>
    public static class WithLazyList
    {
        /// <summary>Implements the show case.</summary>
        public static void Show()
        {
            Console.WriteLine("(6) Use Lazy loading for student list");
            Console.WriteLine("-------------------------------------");

            Class c = Orm.GetObject<Class>("c.0");
            c.Students.Add(Orm.GetObject<Student>("s.0"));
            c.Students.Add(Orm.GetObject<Student>("s.1"));

            Orm.SaveObject(c);

            c = Orm.GetObject<Class>("c.0");

            Console.WriteLine("Students in " + c.Name + ":");
            foreach(Student i in c.Students)
            {
                Console.WriteLine(i.FirstName + " " + i.Name);
            }

            Console.WriteLine("\n");
        }
    }
}
