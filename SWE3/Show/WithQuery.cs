using System;
using SWE3.ExampleProject.School;
using SWE3.ORM;

namespace SWE3.ExampleProject.Show
{
    /// <summary>This show case demonstrates a query.</summary>
    public static class WithQuery
    {
        /// <summary>Implements the show case.</summary>
        /* public static void Show()
         {
             Console.WriteLine("(7) Query demonstration");
             Console.WriteLine("-----------------------");
 
             Console.WriteLine("Students with grade > 1:");
             foreach(Student i in Mapper.From<Student>().GreaterThan("Grade", 1))
             {
                 Console.WriteLine(i.FirstName + " " + i.Name);
             }
 
             Console.WriteLine("\nStudents with grade > 1 or firstname starts with 'al':");
             foreach(Student i in Mapper.From<Student>().GreaterThan("Grade", 1).Or().Like("FirstName", "al%", true))
             {
                 Console.WriteLine(i.FirstName + " " + i.Name);
             }
 
             Console.WriteLine("\nShow all persons:");
             foreach(Person i in Mapper.From<Person>())
             {
                 Console.WriteLine(i.FirstName + " " + i.Name + " (" + i.GetType().Name + ")");
             }
         }
     }*/
    }
}
