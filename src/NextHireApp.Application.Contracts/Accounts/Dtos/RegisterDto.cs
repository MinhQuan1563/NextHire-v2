using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NextHireApp.Accounts.Dtos
{
    public class RegisterDto : IValidatableObject
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)] 
        public string Password { get; set; }

        [Required] 
        public string UserName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext _) { yield break; }
    }
}
