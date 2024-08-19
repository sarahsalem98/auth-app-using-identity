using AuthAppBusiness.Constants;
using AuthAppDataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AuthApp.PageOperationAuthorization
{
    public class PageOperationHandler : AuthorizationHandler<PageOperationRequirement>
    {
        private readonly UserManager<User> _userManger;
        private readonly RoleManager<Role> _roleManger;

        public PageOperationHandler(UserManager<User> userManager,RoleManager<Role> roleManager )
        {
            _userManger = userManager;
            _roleManger = roleManager;  
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PageOperationRequirement requirement)
        {
            var user= await _userManger.GetUserAsync(context.User);
            if(user == null) {
                return;
            }
            var roles = await _userManger.GetRolesAsync(user);
            var rolesClaims = new List<Claim>();
            foreach (var role in roles)
            {

                var roleobj = await _roleManger.FindByNameAsync(role);
                var claims=await _roleManger.GetClaimsAsync(roleobj);   
                rolesClaims.AddRange(claims);   
            }
            var parsedUserType = (int)((UserType)Enum.Parse(typeof(UserType), requirement.UserType));
            var sameUserType = user.Type == parsedUserType;
            var hasRequiredClaim = true;
            var roleType = "";
            if (!string.IsNullOrEmpty(requirement.Operation)&& !string.IsNullOrEmpty(requirement.Page))
            {
                var parsedOperation = (int)((Operations)Enum.Parse(typeof(Operations), requirement.Operation));
                var parsedPage = 0;
                if (user.Type != null && user.Type!=(int)UserType.SuperAdmin)
                {
                    if (parsedUserType == (int)UserType.Employee)
                    {
                        roleType = RoleType.Employee.ToString();
                        parsedPage = (int)((Employee_Pages)Enum.Parse(typeof(Employee_Pages), requirement.Page));

                    }
                    else if(parsedUserType == (int)UserType.Doctor)
                    {
                        roleType = RoleType.Doctor.ToString();
                        parsedPage = (int)((Doctor_Pages)Enum.Parse(typeof(Doctor_Pages), requirement.Page));
                    }

                }
                hasRequiredClaim = rolesClaims.Any(c => c.Type == "Permission."+roleType &&
                  c.Value == $"{parsedPage}:{parsedOperation}");
            }

            if (hasRequiredClaim && sameUserType)
            {
                context.Succeed(requirement);
            }
        }
    }
}
