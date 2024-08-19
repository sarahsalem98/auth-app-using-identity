using System.ComponentModel.DataAnnotations;

namespace AuthApp.Areas.Admin.Models
{
    public class AuthMV
    {
    }

    public class LoginMV
    {
        [Required (ErrorMessage ="Email is Required")]
        public string Email { get; set; }
        [Required( ErrorMessage ="password is required")]
        public string Password { get; set; }    
    }
}
