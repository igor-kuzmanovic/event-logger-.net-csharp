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
        public static string Encrypt(string key, X509Certificate2 certificate)
        {
            byte[] keyData = Encoding.Unicode.GetBytes(key);
            key = string.Empty;

            byte[] encryptedKeyData = Encrypt(keyData, certificate);
            string encryptedKey = encryptedKeyData.ToString();
            Array.Clear(encryptedKeyData, 0, encryptedKeyData.Length);

            return encryptedKey;
        }

        public static byte[] Encrypt(byte[] keyData, X509Certificate2 certificate)
        {
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)certificate.PrivateKey;
            if (csp == null)
            {
                throw new Exception("Unable to obtain the private key from the provided certificate.");
            }

            byte[] encryptedKeyData = csp.Encrypt(keyData, false);
            Array.Clear(keyData, 0, keyData.Length);

            return encryptedKeyData;
        }

        public static string Decrypt(string encryptedKey, X509Certificate2 certificate)
        {
            byte[] encryptedKeyData = Encoding.Unicode.GetBytes(encryptedKey);
            encryptedKey = string.Empty;

            byte[] keyData = Decrypt(encryptedKeyData, certificate);
            string key = keyData.ToString();
            Array.Clear(encryptedKeyData, 0, encryptedKeyData.Length);

            return key;
        }

        public static byte[] Decrypt(byte[] encryptedKeyData, X509Certificate2 certificate)
        {
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)certificate.PublicKey.Key;
            if (csp == null)
            {
                throw new Exception("Unable to obtain the public key from the provided certificate.");
            }

            byte[] decryptedKeyData = csp.Decrypt(encryptedKeyData, false);
            Array.Clear(encryptedKeyData, 0, encryptedKeyData.Length);

            return decryptedKeyData;
        }
    }
}
