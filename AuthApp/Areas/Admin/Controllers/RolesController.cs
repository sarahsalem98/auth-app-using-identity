
using AuthApp.Areas.Admin.Models;
using AuthApp.Models;
using AuthApp.Services;
using AuthAppBusiness.Constants;
using AuthAppDataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = RolesConstants.SuperAdmin)]
    public class RolesController : Controller
    {
        private readonly RoleManager<Role> roleManager;
        private readonly RoleMangerService roleMangerService;
        public RolesController(RoleManager<Role> roleManager,RoleMangerService roleMangerService)
        {
            this.roleManager = roleManager;
            this.roleMangerService = roleMangerService;
                
        }
        public IActionResult Index(string Type )
        {
            int parsedRole = (int)((AuthAppBusiness.Constants.RoleType)Enum.Parse(typeof(RoleType), Type));
            var Roles = roleManager.Roles.Where(r=>r.Type==parsedRole).Select(role => new RoleMV
            {
                Id = role.Id,
                Name = role.Name
            }).ToList();

            var rolesMV = new RolesMV
            {
                Type = Type,
                Roles = Roles
            };
            return View(rolesMV);
        }


        [HttpGet]
        public async  Task<IActionResult> AddEditRolePermissions(string Type,int roleId)
        {
            var roleClaims = new List<RoleClaims>();
            var Role = new Role();
            if (roleId > 0)
            {
                roleClaims =  roleManager.GetClaimsAsync(new Role { Id = roleId }).Result.Select(c=>new RoleClaims
                {
                    ClaimType=c.Type,
                    ClaimValue=c.Value  
                }).ToList();
                Role=  roleManager.Roles.FirstOrDefault(r => r.Id == roleId);    
            }

            var Pages = Type == RoleTypeConstants.Employee ? PageOperationsConstant.EmployeePageOperations
                           .Select(kvp => new PageOperationsMV
                           {
                               PageNo = (int)kvp.Key,
                               PageName = kvp.Key.ToString(),
                               Operations = kvp.Value.Select(op => new OperationsMV
                               {
                                   OperationNo = (int)op,
                                   Name = op.ToString()
                               ,
                                   IsChecked = roleClaims.Any(rc =>
                                   {
                                       var claimPart = rc.ClaimValue.Split(":");
                                       return claimPart.Length == 2 &&
                                              claimPart[0] == ((int)kvp.Key).ToString() &&
                                              claimPart[1] == ((int)op).ToString();
                                   })
                               }).ToList()
                           }).ToList()
                           : PageOperationsConstant.DoctorPageOperations
                           .Select(kvp => new PageOperationsMV
                           {
                               PageNo = (int)kvp.Key,
                               PageName = kvp.Key.ToString(),
                               Operations = kvp.Value.Select(op => new OperationsMV { OperationNo = (int)op, Name = op.ToString()
                               ,
                                   IsChecked = roleClaims.Any(rc =>
                                   {
                                       var claimPart = rc.ClaimValue.Split(":");
                                       return claimPart.Length == 2 &&
                                              claimPart[0] == ((int)kvp.Key).ToString() &&
                                              claimPart[1] == ((int)op).ToString();
                                   })
                               }).ToList()
                           }).ToList();
            var superAdminPermissions = new SuperAdminPermissionsMV
            {
                PagesMV = Pages
            };
            var rolesPermissionsMv = new RolesPermissionsMV
            {
                Type = Type,
                RoleMV=new RoleMV { Id=roleId,Name=Role.Name},
                SuperAdminPermissionsMV = superAdminPermissions
            };

            return PartialView("_AddEditRolePermissions", rolesPermissionsMv);
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRolePermissions(AddRolePermission addRolePermission)
        {

            var res= await roleMangerService.CreateRoleWithClaimsAsync(addRolePermission.roleName,addRolePermission.type, addRolePermission.permissions);
            var result = new ResultMV<string>();

            result = new ResultMV<string>()
            {
                Data = res.ToString(),
                Status = res>0? Status.Success:Status.Error,
                ServerInfo = new ServerInfo
                {
                    CustomeStatusCode = res>0? CustomeStatusCodes.Success:CustomeStatusCodes.InternalServerError,
                    Message = res>0? "permissions added successfully":"something went wrong"
                }
            };
            return Ok(result);    
        }
    }
}
