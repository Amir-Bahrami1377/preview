namespace FormerUrban_Afta.DataAccess.DTOs.Login
{
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }


        public AuthResponse IsSuccess(string userName = "")
        {
            Success = true;
            UserName = userName;
            return this;
        }

        public AuthResponse IsFailed(string message, string userName = "")
        {
            Success = false;
            Message = message;
            UserName = userName;
            return this;
        }

        //public string Id { get; set; }
        //public string Email { get; set; }
        //public string Token { get; set; }
    }
}
