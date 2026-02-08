namespace FormerUrban_Afta.DataAccess.Services.Interfaces;
public interface IEncryptionService
{
    Task<string> EncryptAsync(string plaintext);
    Task<string> DecryptAsync(string ciphertext);
}


