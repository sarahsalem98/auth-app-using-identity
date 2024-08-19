using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDataAccess.Models
{
    public class User:IdentityUser<int>
    {
        public string? FirstName { get; set; }   
        public string? LastName { get; set; }
        public string ?Gender { get; set; }
        public int? Age { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int ? Type { get; set; } 
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
