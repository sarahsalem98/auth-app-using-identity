using System.Reflection.PortableExecutable;

namespace AuthApp.Areas.Admin.Models
{

    public class OperationsMV
    {
        public int OperationNo { get; set; } 
        public string Name { get; set; }
        public Boolean IsChecked { get; set; }    
    }

    public class PageOperationsMV
    {
        public int PageNo { get; set; } 
        public string PageName { get; set; }
        public List<OperationsMV> Operations { get; set; }    
    }
    public class RoleMV
    {
        public int Id { get; set; }  
        public string Name { get; set; }

    }

    public class RolesMV
    {
       public string Type { get; set; } 
       public List<RoleMV> Roles { get; set; }
    }
    public class SuperAdminPermissionsMV
    {
        public RoleMV CurrentRole { get; set; }    
        public List<PageOperationsMV> PagesMV { get; set; }
  
    }
    public class RolesPermissionsMV
    {
        public string Type { get; set; }    
        public RoleMV RoleMV { get; set; } 
        public SuperAdminPermissionsMV SuperAdminPermissionsMV { get; set; }
    }



/// <summary>
///  for adding rolePermission
/// </summary>
    public class AddRolePermission
    {
        public string type { get; set; }    
        public string roleId { get; set; } 
        public string roleName { get; set; }    
        public List<Permissions> permissions { get; set; }  

    }
    public class Permissions
    {
        public int pageNo { get; set; }
        public int operationNo { get; set; }
    }


}
