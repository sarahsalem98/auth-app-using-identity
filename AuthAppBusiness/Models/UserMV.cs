

namespace AuthAppBusiness.Models
{

    public class UserSearchMV
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
        public int Draw { get; set; }
    }

    public class UserMV
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string phoneNumber { get; set; }
        public byte Gender { get; set; }
        public string Address { get; set; }
        public string? SelectedRole { get; set; }
        public int ? Type { get; set; } 
        public List<RolesMV> Roles { get; set; }    
    }
    public class ListUsersMV
    {
        public int? ListCount { get; set; }
        public List<UserMV>? UsersList { get; set; }

    }
}
