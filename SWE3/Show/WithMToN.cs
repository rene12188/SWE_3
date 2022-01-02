using System;
using SWE3.ExampleProject.School;
using SWE3.ORM;

namespace SWE3.ExampleProject.Show
{
    /// <summary>This show case uses an m:n relationship.</summary>
    public static class WithMToN
    {
        /// <summary>Implements the show case.</summary>
        public static void Show()
        {
            Console.WriteLine("(5) Create and load an object with m:n");
            Console.WriteLine("--------------------------------------");

            Course c = new Course();
            c.ID = "x.0";
            c.Name = "Demons 1";
            c.Teacher = Mapper.Get<Teacher>("t.0");

            Student s = new Student();
            s.ID = "s.0";
            s.Name = "Aalo";
            s.FirstName = "Alice";
            s.Gender = Gender.FEMALE;
            s.BirthDate = new DateTime(1990, 1, 12);
            s.Grade = 1;
            Mapper.SaveObject(s);

            c.Students.Add(s);

            s = new Student();
            s.ID = "s.1";
            s.Name = "Bumblebee";
            s.FirstName = "Bernard";
            s.Gender = Gender.MALE;
            s.BirthDate = new DateTime(1991, 9, 23);
            s.Grade = 2;
            Mapper.SaveObject(s);

            c.Students.Add(s);

            Mapper.SaveObject(c);

            c = Mapper.Get<Course>("x.0");

            Console.WriteLine("Students in " + c.Name + ":");
            foreach(Student i in c.Students)
            {
                Console.WriteLine(i.FirstName + " " + i.Name);
            }

            Console.WriteLine("\n");
        }
    }
}
