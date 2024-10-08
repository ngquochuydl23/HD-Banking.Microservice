using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HD.Wallet.Shared.Utils
{
    public static class AesDecryption
    {
        public static byte[] HashKey(string key)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
            }
        }

        // Decrypt the encrypted data using AES key
        public static string Decrypt(string encryptedData)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);

            using (Aes aes = Aes.Create())
            {
                aes.Key = HashKey("6c379387-1bfd-40cf-85b9-0b309a80a8a4");
                aes.Mode = CipherMode.ECB; // Using ECB mode as a simple example
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }
    }
}
