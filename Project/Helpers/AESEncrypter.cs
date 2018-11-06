using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class AESEncrypter
    {
        public static byte[] Encrypt(byte[] valueData, byte[] keyData)
        {
            string value = Encoding.Unicode.GetString(valueData);
            Array.Clear(valueData, 0, valueData.Length);

            byte[] encryptedValueDataWithIV = Encrypt(value, keyData);
            value = string.Empty;
            Array.Clear(keyData, 0, keyData.Length);

            return encryptedValueDataWithIV;
        }

        public static byte[] Encrypt(string value, byte[] keyData)
        {
            byte[] encryptedValueData = null;
            byte[] encryptedValueDataWithIV = null;

            using (AesCryptoServiceProvider csp = new AesCryptoServiceProvider())
            {
                csp.Mode = CipherMode.CBC;

                csp.Key = keyData;
                Array.Clear(keyData, 0, keyData.Length);

                csp.GenerateIV();

                using (ICryptoTransform encryptor = csp.CreateEncryptor())
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                            {
                                streamWriter.Write(value);
                            }

                            encryptedValueData = memoryStream.ToArray();
                        }
                    }
                }

                encryptedValueDataWithIV = new byte[encryptedValueData.Length + csp.IV.Length];
                Array.Copy(csp.IV, 0, encryptedValueDataWithIV, 0, csp.IV.Length);
                Array.Copy(encryptedValueData, csp.IV.Length + 1, encryptedValueDataWithIV, 0, encryptedValueData.Length);
                Array.Clear(encryptedValueData, 0, encryptedValueData.Length);
            }

            value = string.Empty;
            Array.Clear(keyData, 0, keyData.Length);

            return encryptedValueDataWithIV;
        }

        public static string Decrypt(string encryptedValueWithIV, byte[] keyData)
        {
            byte[] encryptedValueDataWithIV = Encoding.Unicode.GetBytes(encryptedValueWithIV);
            encryptedValueWithIV = string.Empty;

            string value = Decrypt(encryptedValueDataWithIV, keyData);
            Array.Clear(encryptedValueDataWithIV, 0, encryptedValueDataWithIV.Length);
            Array.Clear(keyData, 0, keyData.Length);

            return value;
        }

        public static string Decrypt(byte[] encryptedValueDataWithIV, byte[] keyData)
        {
            string value = string.Empty;
            byte[] encryptedValueData = null;

            using (AesCryptoServiceProvider csp = new AesCryptoServiceProvider())
            {
                csp.Mode = CipherMode.CBC;

                csp.Key = keyData;
                Array.Clear(keyData, 0, keyData.Length);

                csp.IV = new byte[csp.BlockSize / 8];
                Array.Copy(encryptedValueDataWithIV, 0, csp.IV, 0, csp.IV.Length);

                encryptedValueData = new byte[encryptedValueDataWithIV.Length - csp.IV.Length];
                Array.Copy(encryptedValueDataWithIV, 0, encryptedValueData, 0, encryptedValueData.Length);
                Array.Clear(encryptedValueDataWithIV, 0, encryptedValueDataWithIV.Length);

                using (ICryptoTransform decryptor = csp.CreateDecryptor())
                {
                    using (MemoryStream memoryStream = new MemoryStream(encryptedValueDataWithIV))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader(cryptoStream))
                            {
                                value = streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }

            Array.Clear(encryptedValueData, 0, encryptedValueData.Length);
            Array.Clear(keyData, 0, keyData.Length);

            return value;
        }
    }
}
