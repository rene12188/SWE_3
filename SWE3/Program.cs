using System;
using System.Reflection;
using System.Reflection;
using Npgsql.PostgresTypes;


namespace SWE3.Demo.SampleApp
{
    /// <summary>This is the main program class for this sample application.</summary>
    class Program
    {
       
        static void Main(string[] args)
        {
            object tmp = new Student();
            Type type = tmp.GetType();
            var props = type.GetProperties();

            Console.WriteLine(type);

            Console.WriteLine(type.BaseType);
            foreach (var prop in props)
            {
                Console.WriteLine("Type: " + prop.PropertyType.ToString() + "Name: " + prop.Name);
                
                
            }

        }

      
        }
    }

