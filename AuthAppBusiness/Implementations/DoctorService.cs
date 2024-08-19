using AuthApp;
using AuthAppBusiness.Constants;
using AuthAppBusiness.Interfaces;
using AuthAppBusiness.Models;
using AuthAppDataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppBusiness.Implementations
{
    public class DoctorService : IDoctor
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        public DoctorService(ApplicationDbContext dbContext,IServiceProvider serviceProvider )
        {
            _context = dbContext;   
            _serviceProvider = serviceProvider; 
                
        }
        public int AddDoctor(Doctor doctor)
        {
            int result= 0;  
            var roleManger = _serviceProvider.GetRequiredService<RoleManager<Role>>();
            var userManger = _serviceProvider.GetRequiredService<UserManager<User>>();

            var user = _context.Users.FirstOrDefault(u => u.Id == doctor.UserId);
            if(user != null) {
              
            
                    _context.Doctors.Add(doctor);
                    _context.SaveChanges();
                    result = doctor.Id;

           
            }
            return result;
        }

        public Tuple<int,List<Doctor>> GetDoctors(DoctorSearchMV doctorSearchMV)
        {

            var query = _context.Doctors.Where(item =>
           (string.IsNullOrEmpty(doctorSearchMV.Name) || item.User.FirstName != null && item.User.FirstName.Contains(doctorSearchMV.Name))
         && (doctorSearchMV.Id == 0 || item.Id == doctorSearchMV.Id)
         && (string.IsNullOrEmpty(doctorSearchMV.Email) || item.User.Email.Contains(doctorSearchMV.Email))
         && (string.IsNullOrEmpty(doctorSearchMV.PhoneNumber) || item.User.PhoneNumber.Contains(doctorSearchMV.PhoneNumber))
         && (doctorSearchMV.RoleId == 0 || _context.UserRoles.Any(u => u.UserId == item.UserId && u.RoleId == doctorSearchMV.RoleId))
            )
                .Include(d => d.User)
                .ThenInclude(d => d.UserRoles)
                .ThenInclude(d => d.Role);
             var count = query.Count();
             var doctors=query.Skip(doctorSearchMV.Start).Take(doctorSearchMV.Length).ToList();
              
            return new Tuple<int,List<Doctor>>(count,doctors);   
        }

        public Doctor GetDoctorById(int doctorId)
        {
            return _context.Doctors.Include(d=>d.User).FirstOrDefault(d => d.Id == doctorId);
        }
         public  int DoctorsCount()
        {
            return _context.Doctors.Count();    
        }
        public int UpdateDoctorSelectedRole(int doctorUserId, string selectedRole)
        {
            var userManger = _serviceProvider.GetRequiredService<UserManager<User>>();
            var res = 0;
            var user = _context.Users.FirstOrDefault(u => u.Id == doctorUserId);
            if (user != null && !string.IsNullOrEmpty(selectedRole)) {
            
                var result=userManger.AddToRoleAsync(user, selectedRole).Result;
                if(result.Succeeded) {
                    res = doctorUserId;
                }

            }
            return res; 

        }


    }
}
