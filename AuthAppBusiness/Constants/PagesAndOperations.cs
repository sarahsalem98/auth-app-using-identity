using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppBusiness.Constants
{
   public enum Employee_Pages
    {
        Dashboard=1,
        Roles=2,
        Doctors=3,
        Patients=4
    }
    public enum Doctor_Pages
    {
        Dashboard = 1

    }
    public enum Patient_Pages { 
    
    }

    public enum Operations
    {
        Index=1,
        Add=2,
        Update=3,
        Delete=4,
        
    }

    public static class PageOperationsConstant
    {
        public static readonly Dictionary<Employee_Pages, List<Operations>> EmployeePageOperations = new Dictionary<Employee_Pages, List<Operations>>()
        {
        { Employee_Pages.Dashboard, new List<Operations> { Operations.Index } },
        { Employee_Pages.Roles, new List<Operations> { Operations.Index, Operations.Add, Operations.Update, Operations.Delete } },
        { Employee_Pages.Doctors, new List<Operations> { Operations.Index, Operations.Add, Operations.Update, Operations.Delete } },
        { Employee_Pages.Patients, new List<Operations> { Operations.Index, Operations.Add, Operations.Update, Operations.Delete } }
        };

        public static readonly Dictionary<Doctor_Pages, List<Operations>> DoctorPageOperations = new Dictionary<Doctor_Pages, List<Operations>>()
        {
            {Doctor_Pages.Dashboard,new List<Operations>{Operations.Index} }
        };
    }


}
