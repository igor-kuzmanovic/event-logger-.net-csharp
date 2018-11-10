using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Helpers
{
    public static class RSAEncrypter
    {
        public static byte[] Encrypt(string text, X509Certificate2 certificate)
        {
            RSACryptoServiceProvider csp = certificate.PublicKey.Key as RSACryptoServiceProvider;
            return csp.Encrypt(Encoding.ASCII.GetBytes(text), true);
        }

        public static string Decrypt(byte[] encryptedText, X509Certificate2 certificate)
        {
            RSACryptoServiceProvider csp = certificate.PrivateKey as RSACryptoServiceProvider;
            return Encoding.ASCII.GetString(csp.Decrypt(encryptedText, true));
        }
    }
}
