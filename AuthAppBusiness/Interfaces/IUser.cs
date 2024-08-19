using AuthAppBusiness.Models;
using AuthAppDataAccess.Models;
using AuthAppDataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppBusiness.Interfaces
{
    public interface IUser
    {
        public int AddUser(User user,string password,List<Role> roles=null);
        public User GetUserById(int id);  
        public int UpdateUser (User user, List<Role> roles=null);
        public Tuple<int, List<User>> GetUsers(UserSearchMV userSearchMV);
        public int UsersCount();
        public int UpdateRoles(int UserId, List<Role> Roles);


    }
}
