namespace FormerUrban_Afta.DataAccess.DTOs.Login
{
    public class AuthRequest
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا نام کاربری را وارد کنید!")]
        public string UserName { get; set; }

        [Display(Name = "رمز عبور ")]
        [Required(ErrorMessage = "لطفا رمز عبور را وارد کنید!")]
        public string Password { get; set; }

        public string Mobile { get; set; }
        public int Sms { get; set; }
        public string Code { get; set; }
        public string CaptchaCode { get; set; }
        public string? ExpireTime { get; set; }
        public List<string>? message { get; set; }
    }
}
