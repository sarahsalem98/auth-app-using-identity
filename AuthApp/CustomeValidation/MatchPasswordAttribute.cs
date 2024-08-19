using AuthApp.Areas.Doctor.Models;
using System.ComponentModel.DataAnnotations;

namespace AuthApp.CustomeValidation
{
    public class MatchPasswordAttribute:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,ValidationContext validationContext)
        {
            var registerDoctor = (RegisterMV)validationContext.ObjectInstance;
            if (registerDoctor.Password != registerDoctor.ConfirmedPassword) {
                return new ValidationResult(ErrorMessage ?? "The password and confirmation password do not match.");
            }
            return ValidationResult.Success;
        }   
    }
}
