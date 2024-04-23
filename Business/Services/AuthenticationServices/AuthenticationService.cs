using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.AuthenticationServices
{
    public class AuthenticationService
    {


        public AuthenticationService()
        { 
        
        }

        public string EncryptString(string plainText, byte[] key, byte[] iv)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cs.FlushFinalBlock();
                    }
                    byte[] cipherTextBytes = ms.ToArray();
                    return Convert.ToBase64String(cipherTextBytes);
                }

            }
        
        }


    }
}
