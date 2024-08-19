using AuthApp.CustomeValidation;
using System.ComponentModel.DataAnnotations;

namespace AuthApp.Areas.Doctor.Models
{
    public class AuthMV
    {
    }

    public class LoginMV
    {
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "password is required")]
        public string Password { get; set; }
    }

    public class RegisterMV
    {
 
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "ConfirmedPassword is Required")]
        [MatchPassword(ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmedPassword { get; set; }
        [Required(ErrorMessage = "FirstName is Required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Lastname is Required")]
        public string LastName { get; set; }   
        public int Age { get; set; }    
        public string phoneNumber { get; set; } 
        public byte Gender { get; set; } 
        public string UniversityDegree { get; set; }  
        public string Address { get; set; } 
        
    }
}
