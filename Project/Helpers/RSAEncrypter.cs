using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class RSAEncrypter
    {
        public static byte[] Encrypt(byte[] valueData, X509Certificate2 certificate)
        {
            byte[] encryptedValueData = null;

            using (RSACryptoServiceProvider csp = certificate.PublicKey.Key as RSACryptoServiceProvider)
            {
                encryptedValueData = csp.Encrypt(valueData, false);
                Array.Clear(valueData, 0, valueData.Length);
            }

            return encryptedValueData;
        }

        public static byte[] Encrypt(string value, X509Certificate2 certificate)
        {
            byte[] valueData = Encoding.Unicode.GetBytes(value);
            value = string.Empty;

            byte[] encryptedValueData = Encrypt(valueData, certificate);
            Array.Clear(valueData, 0, valueData.Length);

            return encryptedValueData;
        }

        public static string Decrypt(byte[] encryptedValueData, X509Certificate2 certificate)
        {
            string value = null;

            using (RSACryptoServiceProvider csp = certificate.PrivateKey as RSACryptoServiceProvider)
            {
                byte[] valueData = csp.Decrypt(encryptedValueData, false);
                Array.Clear(encryptedValueData, 0, encryptedValueData.Length);

                value = Encoding.Unicode.GetString(valueData);
                Array.Clear(valueData, 0, valueData.Length);
            }

            return value;
        }

        public static string Decrypt(string encryptedValue, X509Certificate2 certificate)
        {
            byte[] encryptedValueData = Encoding.Unicode.GetBytes(encryptedValue);
            encryptedValue = string.Empty;

            string value = Decrypt(encryptedValueData, certificate);
            Array.Clear(encryptedValueData, 0, encryptedValueData.Length);

            return value;
        }
    }
}
