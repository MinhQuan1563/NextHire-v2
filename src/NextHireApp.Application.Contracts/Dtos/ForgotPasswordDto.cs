namespace NextHireApp.Dtos
{
    public class ForgotPasswordDto
    {
        public string Email { get; set; } = default!;
    }

    public class ResetPasswordDto
    {
        public string UserId { get; set; } = default!;
        public string ResetToken { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
    }

    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
    }
}
