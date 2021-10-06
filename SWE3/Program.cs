using System;
using System.Reflection;
using System.Reflection;
using Npgsql;
using Npgsql.PostgresTypes;
using SWE3.ORM;


namespace SWE3.Demo.SampleApp
{
    /// <summary>This is the main program class for this sample application.</summary>
    class Program
    {
       
        static void Main(string[] args)
        {
            NpgsqlConnection  conn =
                new NpgsqlConnection(
                    "Server = 127.0.0.1; Port = 5432; Database = swe3; User Id = postgres; Password = a");
            Mapper map = new Mapper(conn);
            Student tmp = new Student();
            tmp.Grade = 1;
            tmp.Name = "Mr Placeholder";
            tmp.BirthDate = new DateTime(2021, 9, 17);
            tmp.FirstName = "John";
            tmp.ID = "if19b09888";

            tmp.Gender = Gender.MALE;
            map.SaveObject(tmp);

            /*  Type type = tmp.GetType();
              var props = type.GetProperties();
              var basetype = type.BaseType;
              Console.WriteLine(type);
  
              Console.WriteLine(basetype);
              Console.WriteLine(basetype.BaseType);
              foreach (var prop in props)
              {
                  Console.WriteLine("Type: " + prop.PropertyType.ToString() + "Name: " + prop.Name);
                  
                  
              }*/

        }

      
        }
    }

