using System;
using SWE3.ExampleProject.School;
using SWE3.ORM;

namespace SWE3.ExampleProject.Show.OLD
{
    public static class Class1
    {
       public static void SaveTeacher()
        {
            Teacher tmp = new Teacher();
            tmp.Salary = 1;
            tmp.Name = "Mr Placeholder";
            tmp.BirthDate = new DateTime(2021, 9, 17);
            tmp.HireDate = new DateTime(2021, 9, 17);
            tmp.FirstName = "John";
            tmp.ID = "if19b09888";


            Mapper.SaveObject(tmp);
        }
    }
}
