using System;
using SWE3.ExampleProject.School;
using SWE3.ORM;

namespace SWE3.ExampleProject.Show
{
    /// <summary>This show case uses lazy loading.</summary>
    public static class WithLazyList
    {
        /// <summary>Implements the show case.</summary>
        public static void Show()
        {
            Console.WriteLine("(6) Use Lazy loading for student list");
            Console.WriteLine("-------------------------------------");

            Class c = Mapper.Get<Class>("c.0");
            c.Students.Add(Mapper.Get<Student>("s.0"));
            c.Students.Add(Mapper.Get<Student>("s.1"));

            Mapper.SaveObject(c);

            c = Mapper.Get<Class>("c.0");

            Console.WriteLine("Students in " + c.Name + ":");
            foreach(Student i in c.Students)
            {
                Console.WriteLine(i.FirstName + " " + i.Name);
            }

            Console.WriteLine("\n");
        }
    }
}
