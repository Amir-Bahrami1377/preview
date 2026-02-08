namespace FormerUrban_Afta.DataAccess.DTOs.Login;
public class TwoFactorAuthenticationDto
{
    public bool HasAuthenticator { get; set; }
    public bool Is2faEnabled { get; set; }
    public bool IsMachineRemembered { get; set; }
    public int RecoveryCodesLeft { get; set; }
}

