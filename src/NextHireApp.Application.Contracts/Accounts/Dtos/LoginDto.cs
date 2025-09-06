using System.ComponentModel.DataAnnotations;

namespace NextHireApp.Accounts.Dtos
{
    public class LoginDto
    {
        [Required] 
        public string UserNameOrEmail { get; set; }

        [Required] 
        public string Password { get; set; }
    }
}
