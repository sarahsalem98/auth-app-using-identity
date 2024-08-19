using AuthAppDataAccess.Models;

namespace AuthAppBusiness.Models
{
    public class DoctorMV
    {
        public int? Id { get; set; } 
        public int? UserId { get; set; } 
        public string? UniversityDegree { get; set; }
        public string? Address { get; set; }
   
        public UserMV User { get; set; }    
    }

    public class ListDoctorsMV
    {
        public int? ListCount { get; set; }  
        public List<DoctorMV>? DoctorsList { get; set; }

    }

    public class DoctorSearchMV
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; } 
        public int RoleId { get; set; }  
        public string? Email { get; set; }

        public int Page { get; set; }
        public int PageSize { get; set; }   

        public int Start { get; set; }  
        public int Length { get; set; } 

        public int Draw { get;set; }
    }
}
