using System;
using SWE3.ExampleApp.School;
using SWE3.ORM;

namespace SWE3.ExampleApp.Show
{
    public static class WithQuery
    {
        public static void Show()
        {
            Console.WriteLine("(7) Query demonstration");
            Console.WriteLine("-----------------------");

           Console.WriteLine("Students with grade > 1:");
            foreach(Student i in Orm.From<Student>())
            {
                Console.WriteLine(i.FirstName + " " + i.Name);
            }

            Console.WriteLine("\nStudents with grade > 1 or firstname starts with 'al':");
            foreach(Student i in Orm.From<Student>().GreaterThan("Grade", 1).Or().Like("FirstName", "al%", true))
            {
                Console.WriteLine(i.FirstName + " " + i.Name);
            }

         /*   Console.WriteLine("\nShow all persons:");
            foreach(Person i in Orm.From<Person>())
            {
                Console.WriteLine(i.FirstName + " " + i.Name + " (" + i.GetType().Name + ")");
            }*/
        }
    }
}
