﻿using System;
using System.Threading;
using SWE3.ExampleProject.School;

using SWE3.OrmFramework;

namespace SWE3.ExampleProject.Show
{
    /// <summary>This show case loads and modifies an object and saves it to database.</summary>
    public static class ModifyObject
    {
        /// <summary>Implements the show case.</summary>
        public static void Show()
        {
            Console.WriteLine("(2) Load and modify object");
            Console.WriteLine("--------------------------");
            Mapper.Connection.Close();
            Mapper.Connection.Open();
            Teacher t = Mapper.Get<Teacher>("t.0");
            Thread.Sleep(100);
            Console.WriteLine();
            Console.WriteLine("Salary for " + t.FirstName + " " + t.Name + " is " + t.Salary.ToString() + " Pesos.");

            Console.WriteLine("Give rise of 12000.");
            t.Salary += 12000;

            Console.WriteLine("Salary for " + t.FirstName + " " + t.Name + " is now " + t.Salary.ToString() + " Pesos.");

            Mapper.SaveObject(t);

            Console.WriteLine("\n");
        }
    }
}
