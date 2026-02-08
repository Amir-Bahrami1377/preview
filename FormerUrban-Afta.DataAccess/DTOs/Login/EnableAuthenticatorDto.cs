namespace FormerUrban_Afta.DataAccess.DTOs.Login;

public class EnableAuthenticatorDto
{
    public string SharedKey { get; set; }
    public string AuthenticatorUri { get; set; }
    public string Code { get; set; }
    public byte[] QrCodeImage { get; set; }
    public string UserName { get; set; }
}
