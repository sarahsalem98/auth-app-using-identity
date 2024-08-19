
using Microsoft.AspNetCore.Authorization;

namespace AuthApp.PageOperationAuthorization
{
    public class PageOperationRequirement:IAuthorizationRequirement
    {
        public string Page { get; }
        public string Operation { get; }
        public string UserType { get; } 

        public PageOperationRequirement(string page,string operation,string UserType )
        {
            Page = page;
            Operation = operation;
            this.UserType = UserType;   
            
                
        }


    }
}
