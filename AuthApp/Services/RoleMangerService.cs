using AuthApp.Areas.Admin.Models;
using AuthAppBusiness.Constants;
using AuthAppDataAccess.Models;
using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;

namespace AuthApp.Services
{
    public class RoleMangerService
    {
        private readonly RoleManager<Role> _roleManger;
        public RoleMangerService( RoleManager<Role> roleManager)
        {

            _roleManger = roleManager;   
                
        }
        public async Task<int> CreateRoleWithClaimsAsync/*<TPage,TOperation>*/(string roleName , string roleType, List<Permissions> permissions)
            //where TPage:Enum
            //where TOperation:Enum
        {
            var res = 1;
            var role = await _roleManger.FindByNameAsync(roleName);
            if(role == null)
            {
                role =new Role(roleName)
                {
                    CreatedAt = DateTime.UtcNow,    
                    Type=(int)((RoleType)Enum.Parse(typeof(RoleType), roleType))    
                };   
                var result=await _roleManger.CreateAsync(role);
                if(!result.Succeeded) {
                    res = 0;
                    return res;
                    //throw new Exception("failed to create role");
                }

            }
            else
            {
                if (permissions.Count > 0)
                {
                    var roleCalims = await _roleManger.GetClaimsAsync(role);
                    if(roleCalims.Count>0) {
                        foreach(var claim in  roleCalims) {
                            await _roleManger.RemoveClaimAsync(role, claim);
                        
                        }

                    }
                }
            } 

            foreach(var permission in permissions) {
                 var identityclaim=new RoleClaims
                 {
                     RoleId = role.Id,  
                     ClaimValue= $"{Convert.ToInt32(permission.pageNo)}:{Convert.ToInt32(permission.operationNo)}",
                     ClaimType = $"{"Permission."+roleType}",
                     CreatedAt= DateTime.UtcNow
                 };
                var claimResult = await _roleManger.AddClaimAsync(role, new Claim(identityclaim.ClaimType, identityclaim.ClaimValue));
                if(!claimResult.Succeeded) {
                    res = 0;
                    return res;
                    //throw new Exception($"Failed to add claim: {identityclaim.ClaimValue}");

                }

            }
           return res;

        }
    }
}
