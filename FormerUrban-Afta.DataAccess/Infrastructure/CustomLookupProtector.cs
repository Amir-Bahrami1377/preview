using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormerUrban_Afta.DataAccess.Infrastructure;
public class CustomLookupProtector : ILookupProtector
{

    byte[] iv = { 208, 148, 29, 187, 168, 51, 181, 178, 137, 83, 40, 13, 28, 177, 131, 248 };
    public string Protect(string keyId, string data)
    {
        if (!string.IsNullOrWhiteSpace(data))
            return string.Empty;

        byte[] plainTextBytes = Encoding.UTF8.GetBytes(data);

        string cipherText;
        using (SymmetricAlgorithm algorithm = Aes.Create())
        {
            using (ICryptoTransform encryptor = algorithm.CreateEncryptor(Encoding.UTF8.GetBytes(keyId), iv))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.Close();
                        byte[] chiperTextByte = ms.ToArray();
                        cipherText = Convert.ToBase64String(chiperTextByte);
                    }
                }
            }
        }

        return cipherText;
    }


    public string Unprotect(string keyId, string data)
    {
        byte[] cipherTextBytes = Convert.FromBase64String(data);
        string plainText;
        using (SymmetricAlgorithm algorithm = Aes.Create())
        {
            using (ICryptoTransform decrypter = algorithm.CreateDecryptor(Encoding.UTF8.GetBytes(keyId), iv))
            {
                using (MemoryStream ms = new MemoryStream(cipherTextBytes))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(ms, decrypter, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            plainText = streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        return plainText;
    }
}

