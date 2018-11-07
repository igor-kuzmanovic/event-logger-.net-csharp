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
        public static byte[] Encrypt(string text, X509Certificate2 certificate)
        {
            RSACryptoServiceProvider csp = certificate.PublicKey.Key as RSACryptoServiceProvider;

            byte[] textBytes = Encoding.ASCII.GetBytes(text);
            byte[] encryptedText = csp.Encrypt(textBytes, true);

            return encryptedText;
        }

        public static string Decrypt(byte[] encryptedText, X509Certificate2 certificate)
        {
            RSACryptoServiceProvider csp = certificate.PrivateKey as RSACryptoServiceProvider;

            byte[] textBytes = csp.Decrypt(encryptedText, true);
            string text = Encoding.ASCII.GetString(textBytes);

            return text;
        }
    }
}
