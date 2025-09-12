using Volo.Abp.Auditing;

namespace NextHireApp.Dtos
{
    public class LoginDto
    {
        public string UserNameOrEmail { get; set; } = default!;

        public string Password { get; set; } = default!;
    }
}
