using System.ComponentModel.DataAnnotations;

namespace NextHireApp.Auth.Dtos
{
    public class LoginDto
    {
        [Required] 
        public string UserNameOrEmail { get; set; }

        [Required] 
        public string Password { get; set; }
    }
}
