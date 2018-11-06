using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class RSAKeyEncryption
    {
        public static byte[] Encrypt(byte[] keyData, X509Certificate2 certificate)
        {
            RSACryptoServiceProvider csp = certificate.PublicKey.Key as RSACryptoServiceProvider;

            byte[] encryptedKeyData = csp.Encrypt(keyData, false);
            Array.Clear(keyData, 0, keyData.Length);

            return encryptedKeyData;
        }

        public static string Encrypt(string key, X509Certificate2 certificate)
        {
            byte[] keyData = Encoding.Unicode.GetBytes(key);
            key = string.Empty;

            byte[] encryptedKeyData = Encrypt(keyData, certificate);
            string encryptedKey = Convert.ToBase64String(encryptedKeyData);
            Array.Clear(encryptedKeyData, 0, encryptedKeyData.Length);

            return encryptedKey;
        }

        public static byte[] Decrypt(byte[] encryptedKeyData, X509Certificate2 certificate)
        {
            RSACryptoServiceProvider csp = certificate.PrivateKey as RSACryptoServiceProvider;

            byte[] decryptedKeyData = csp.Decrypt(encryptedKeyData, false);
            Array.Clear(encryptedKeyData, 0, encryptedKeyData.Length);

            return decryptedKeyData;
        }

        public static string Decrypt(string encryptedKey, X509Certificate2 certificate)
        {
            byte[] encryptedKeyData = Convert.FromBase64String(encryptedKey);
            encryptedKey = string.Empty;

            byte[] keyData = Decrypt(encryptedKeyData, certificate);
            string key = Encoding.Unicode.GetString(keyData);
            Array.Clear(encryptedKeyData, 0, encryptedKeyData.Length);

            return key;
        }
    }
}
