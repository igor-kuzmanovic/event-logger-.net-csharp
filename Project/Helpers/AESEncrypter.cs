using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Helpers
{
    public static class AESEncrypter
    {
        public static byte[] Encrypt(string text, string key)
        {
            byte[] encryptedTextWithIV = null;

            using (AesCryptoServiceProvider csp = new AesCryptoServiceProvider())
            {
                csp.Mode = CipherMode.CBC;
                csp.Key = Encoding.ASCII.GetBytes(key).Concat(new byte[32 - Encoding.ASCII.GetByteCount(key)]).ToArray();
                csp.GenerateIV();

                using (ICryptoTransform encryptor = csp.CreateEncryptor())
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                            {
                                streamWriter.Write(text);
                            }

                            byte[] encryptedText = memoryStream.ToArray();
                            encryptedTextWithIV = new byte[encryptedText.Length + csp.IV.Length];
                            Array.Copy(csp.IV, encryptedTextWithIV, csp.IV.Length);
                            Array.Copy(encryptedText, 0, encryptedTextWithIV, csp.IV.Length, encryptedText.Length);
                        }
                    }
                }
            }

            return encryptedTextWithIV;
        }

        public static string Decrypt(byte[] encryptedTextWithIV, string key)
        {
            string text = string.Empty;

            using (AesCryptoServiceProvider csp = new AesCryptoServiceProvider())
            {
                csp.Mode = CipherMode.CBC;
                csp.Key = Encoding.ASCII.GetBytes(key).Concat(new byte[32 - Encoding.ASCII.GetByteCount(key)]).ToArray();
                csp.IV = encryptedTextWithIV.Take(csp.BlockSize / 8).ToArray();

                byte[] encryptedText = new byte[encryptedTextWithIV.Length - csp.IV.Length];
                Array.Copy(encryptedTextWithIV, csp.IV.Length, encryptedText, 0, encryptedText.Length);

                using (ICryptoTransform decryptor = csp.CreateDecryptor())
                {
                    using (MemoryStream memoryStream = new MemoryStream(encryptedText))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader(cryptoStream))
                            {
                                text = streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }

            return text;
        }
    }
}