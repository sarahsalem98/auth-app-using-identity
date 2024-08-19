using AuthAppBusiness.Constants;
using AuthAppDataAccess.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthApp.Seed
{
    public static class SuperAdminSeeder
    {
        public static async Task seedSuperAdimn(IServiceProvider serviceProvider)
        {
            var roleManger = serviceProvider.GetRequiredService<RoleManager<Role>>();
            var userManger = serviceProvider.GetRequiredService<UserManager<User>>();
            var adminUser = new User
            {
                UserName = "SuperAdmin@SuperAdmin.com",
                Email = "SuperAdmin@SuperAdmin.com",
                Type=(int)RoleType.SuperAdmin
            };
            var user = await userManger.FindByEmailAsync(adminUser.Email);
            if (user == null)
            {
                var createUserAdmin = await userManger.CreateAsync(adminUser, "Admin123!");
             

            }
            else
            {
                // Ensure the security stamp is not null
                if (string.IsNullOrEmpty(user.SecurityStamp))
                {
                    user.SecurityStamp = Guid.NewGuid().ToString(); // Set a new security stamp
                    await userManger.UpdateAsync(user);
                }

           

            }



        }
    }
}
