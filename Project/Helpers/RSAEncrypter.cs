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
        public static byte[] Encrypt(string value, X509Certificate2 certificate)
        {
            RSACryptoServiceProvider csp = certificate.PublicKey.Key as RSACryptoServiceProvider;

            byte[] valueData = Encoding.ASCII.GetBytes(value);
            byte[] encryptedValueData = csp.Encrypt(valueData, true);
            Array.Clear(valueData, 0, valueData.Length);

            return encryptedValueData;
        }

        public static string Decrypt(byte[] encryptedValueData, X509Certificate2 certificate)
        {
            RSACryptoServiceProvider csp = certificate.PrivateKey as RSACryptoServiceProvider;

            byte[] valueData = csp.Decrypt(encryptedValueData, true);
            Array.Clear(encryptedValueData, 0, encryptedValueData.Length);

            string value = Encoding.ASCII.GetString(valueData);
            Array.Clear(valueData, 0, valueData.Length);

            return value;
        }
    }
}
