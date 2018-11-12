using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Helpers
{
    public static class AESEncrypter
    {
        public static byte[] Encrypt(string text, byte[] key)
        {
            byte[] encryptedTextWithIV = new byte[0];

            using (AesCryptoServiceProvider csp = new AesCryptoServiceProvider())
            {
                using (SHA256CryptoServiceProvider shaCSP = new SHA256CryptoServiceProvider())
                {
                    // Hash the key to prevent sizing issues
                    csp.Key = shaCSP.ComputeHash(key);
                }

                // Generate an initialization vector
                csp.GenerateIV();

                // Set the operation mode to Cipher Block Chaining for additional security
                csp.Mode = CipherMode.CBC;

                using (ICryptoTransform encryptor = csp.CreateEncryptor())
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                            {
                                // Encrypt the text using the AES algorithm
                                streamWriter.Write(text);
                            }
                        }

                        // Convert the stream content into an array
                        byte[] encryptedText = memoryStream.ToArray();

                        // Create an expanded array to store both the encrypted text and the initialization vector
                        encryptedTextWithIV = new byte[encryptedText.Length + csp.IV.Length];

                        // Copy the initialization vector to the beginning of the array
                        Array.Copy(csp.IV, encryptedTextWithIV, csp.IV.Length);

                        // Append the encrypted text after the initialization vector
                        Array.Copy(encryptedText, 0, encryptedTextWithIV, csp.IV.Length, encryptedText.Length);

                        // Clear the encrypted text array for security reasons
                        Array.Clear(encryptedText, 0, encryptedText.Length);
                    }
                }
            }

            return encryptedTextWithIV;
        }

        public static string Decrypt(byte[] encryptedTextWithIV, byte[] key)
        {
            string text = string.Empty;

            using (AesCryptoServiceProvider csp = new AesCryptoServiceProvider())
            {
                using (SHA256CryptoServiceProvider shaCSP = new SHA256CryptoServiceProvider())
                {
                    // Hash the key to prevent sizing issues
                    csp.Key = shaCSP.ComputeHash(key);
                }

                // Get the initialization vector from the beginning of the encrypted text
                csp.IV = encryptedTextWithIV.Take(csp.BlockSize / 8).ToArray();

                // Set the operation mode to Cipher Block Chaining for additional security
                csp.Mode = CipherMode.CBC;

                // Initialize a new array to store the encrypted text
                byte[] encryptedText = new byte[encryptedTextWithIV.Length - csp.IV.Length];

                // Get the encrypted text from the encrypted text
                Array.Copy(encryptedTextWithIV, csp.IV.Length, encryptedText, 0, encryptedText.Length);

                using (ICryptoTransform decryptor = csp.CreateDecryptor())
                {
                    using (MemoryStream memoryStream = new MemoryStream(encryptedText))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader(cryptoStream))
                            {
                                // Decrypt the encrypted text using the AES algorithm
                                text = streamReader.ReadToEnd();

                                // Clear the encrypted text array for security reasons
                                Array.Clear(encryptedText, 0, encryptedText.Length);
                            }
                        }
                    }
                }
            }

            return text;
        }
    }
}