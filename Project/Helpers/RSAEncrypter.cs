using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Helpers
{
    public static class RSAEncrypter
    {
        public static byte[] Encrypt(string text, X509Certificate2 certificate)
        {
            byte[] encryptedText = null;

            // Grab the public key from the certificate
            RSACryptoServiceProvider csp = certificate.PublicKey.Key as RSACryptoServiceProvider;

            // Convert the text into a byte array
            byte[] textBytes = StringConverter.ToBytes(text);

            // Encrypt the text using the RSA algorithm
            encryptedText = csp.Encrypt(textBytes, true);

            // Clear the text byte array for security reasons
            Array.Clear(textBytes, 0, textBytes.Length);

            return encryptedText;
        }

        public static string Decrypt(byte[] encryptedText, X509Certificate2 certificate)
        {
            string text = string.Empty;

            // Grab the private key from the certificate
            RSACryptoServiceProvider csp = certificate.PrivateKey as RSACryptoServiceProvider;

            // Decrypt the encrypted text using the RSA algorithm
            byte[] textBytes = csp.Decrypt(encryptedText, true);

            // Convert the decrypted text byte array into a string
            text = StringConverter.ToString(textBytes);

            // Clear the encrypted text byte array for security reasons
            Array.Clear(textBytes, 0, textBytes.Length);

            return text;
        }
    }
}
