namespace FormerUrban_Afta.DataAccess.DTOs.IdentityUser
{
    public class ChangePasswordDto
    {
        public string? UserName { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string RepeatPassword { get; set; }

    }
}
