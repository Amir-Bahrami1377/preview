using FormerUrban_Afta.DataAccess.DTOs.Login;
using Zxcvbn;

namespace FormerUrban_Afta.DataAccess.Utilities;

public class ValidationPassword
{
    public static AuthResponse IsValidPassword(string password, string mobile = "", string oldPassword = "")
    {
        var response = new AuthResponse();
        var result = Core.EvaluatePassword(password);

        if (!string.IsNullOrWhiteSpace(oldPassword))
            if (password == oldPassword)
                return response.IsFailed(ValidationMessage.RepeatPassword2());

        if (!string.IsNullOrWhiteSpace(mobile))
            if (password.Contains(mobile))
                return response.IsFailed(ValidationMessage.MobilePassword());

        return password.Length switch
        {
            < 12 => response.IsFailed(ValidationMessage.IsValidMinPassword()),
            > 128 => response.IsFailed(ValidationMessage.IsValidMaxPassword()),
            _ => result.Score < 3 ? response.IsFailed(ValidationMessage.IsValidScorePassword()) : response.IsSuccess()
        };
    }
}

