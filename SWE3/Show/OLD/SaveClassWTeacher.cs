using System;
using SWE3.ExampleProject.School;
using SWE3.OrmFramework;


namespace SWE3.ExampleProject.Show.OLD
{
    public static class SaveClassWTeacher
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

        

            Class newclass = new Class();
            newclass.Name = "Kleine Klasse";
            newclass.Teacher = tmp;
            newclass.ID = "ADS";

            Class newclass2 = new Class();
            newclass2.Name = "Kleine Klasse";
            newclass2.Teacher = tmp;
            newclass2.ID = "ADS2";

            Mapper.SaveObject(newclass);
            Mapper.SaveObject(newclass2);
        }
    }
}
