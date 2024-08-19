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
using System.Text;
using System.Threading.Tasks;

namespace AuthAppBusiness.Implementations
{
    public class UserService : IUser
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        public UserService(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            _context = dbContext;
            _serviceProvider = serviceProvider;

        }
        public int AddUser(User user, string password, List<Role> roles=null)
        {
            int result = 0;
            var userManger = _serviceProvider.GetRequiredService<UserManager<User>>();
            user.UserName = user.Email;
            user.CreatedAt = DateTime.UtcNow;
            var userExisted = userManger.FindByEmailAsync(user.Email).Result;
            if (userExisted == null)
            {
                var createdUser = userManger.CreateAsync(user, password).Result;
                if (createdUser.Succeeded)
                {
                    result= UpdateRoles(user.Id, roles);
                }

            }
            return result;

        }
        public User GetUserById(int id)
        {
            var userManger = _serviceProvider.GetRequiredService<UserManager<User>>();
            var user = userManger.FindByIdAsync(id.ToString()).Result;
            return user;

        }
        public int UpdateUser(User user, List<Role> roles=null)
        {
            int result = 0;
            var userManger = _serviceProvider.GetRequiredService<UserManager<User>>();

            var userExisted = userManger.FindByIdAsync(user.Id.ToString()).Result;
            if (userExisted != null)
            {
                userExisted.Email = user.Email;
                userExisted.FirstName = user.FirstName;
                userExisted.LastName = user.LastName;
                userExisted.PhoneNumber = user.PhoneNumber;
                userExisted.Gender = user.Gender;
                userExisted.SecurityStamp = Guid.NewGuid().ToString();
                userExisted.UserName = user.Email;
                userExisted.UpdatedAt = DateTime.UtcNow;
                result = userManger.UpdateAsync(userExisted).Result.Succeeded ? user.Id : 0;
                result= UpdateRoles(result, roles);  

            }
            return result;

        }
        public Tuple<int, List<User>> GetUsers(UserSearchMV userSearchMV)
        {
            var query = _context.Users.Where(item =>
          (string.IsNullOrEmpty(userSearchMV.Name) || item.FirstName != null && item.FirstName.Contains(userSearchMV.Name))
        && (userSearchMV.Id == 0 || item.Id == userSearchMV.Id)
        && (string.IsNullOrEmpty(userSearchMV.Email) || item.Email.Contains(userSearchMV.Email))
        && (string.IsNullOrEmpty(userSearchMV.PhoneNumber) || item.PhoneNumber.Contains(userSearchMV.PhoneNumber))
        && (userSearchMV.RoleId == 0 || _context.UserRoles.Any(u => u.UserId == item.Id && u.RoleId == userSearchMV.RoleId))
        && (item.Type == (int)UserType.Employee)
           )
               .Include(d => d.UserRoles)
               .ThenInclude(d => d.Role);
            var count = query.Count();
            var users = query.Skip(userSearchMV.Start).Take(userSearchMV.Length).ToList();

            return new Tuple<int, List<User>>(count, users);

        }
        public int UsersCount()
        {
            return _context.Users.Count();
        }
        public int UpdateRoles(int UserId, List<Role> Roles)
        {
            int result = UserId;
            var userManger = _serviceProvider.GetRequiredService<UserManager<User>>();
            var user = userManger.FindByIdAsync(UserId.ToString()).Result;
            if(user!= null && Roles != null && Roles.Count()>0 )
            {
                if (!userManger.GetRolesAsync(user).Result.Contains(Roles.Select(r => r.Name).First()))
                {
                    var res = userManger.AddToRolesAsync(user, Roles.Select(r => r.Name).ToList()).Result;
                    if (res.Succeeded)
                    { 
                        result = UserId;
                    }
                    else
                    {
                        result = 0;
                    }

                }

            }

            return result;

        }
    }
    }
