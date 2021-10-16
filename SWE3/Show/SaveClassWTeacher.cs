using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE3.Demo.SampleApp;
using SWE3.ORM;

namespace SWE3.ExampleProject.Show
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

            tmp.IsMale = 1;

            Class newclass = new Class();
            newclass.Name = "Kleine Klasse";
            newclass.Teacher = tmp;
            newclass.ID = "ADS";

            Mapper.SaveObject(newclass);
        }
    }
}
