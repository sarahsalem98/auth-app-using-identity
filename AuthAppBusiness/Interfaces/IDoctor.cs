using AuthAppBusiness.Models;
using AuthAppDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppBusiness.Interfaces
{
    public interface IDoctor
    {
        public int AddDoctor(Doctor doctor);
        public Tuple<int, List<Doctor>> GetDoctors(DoctorSearchMV doctorSearchMV );
        public Doctor GetDoctorById(int doctorId);
        public int DoctorsCount();
        public int UpdateDoctorSelectedRole(int doctorUserId, string selectedRole);



    }
}
