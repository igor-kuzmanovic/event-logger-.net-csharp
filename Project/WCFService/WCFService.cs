using Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Linq;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WCFServiceCommon;

namespace WCFService
{
    internal class WCFService : IWCFService
    {
        private static SecureString securePrivateKey;

        public string CheckIn()
        {
            X509Certificate2 certificate = ((X509CertificateClaimSet)OperationContext.Current.ServiceSecurityContext.AuthorizationContext.ClaimSets[0]).X509Certificate;

            return RSAKeyEncryption.Encrypt(SecureStringConverter.ToString(securePrivateKey), certificate);
        }

        public void Add()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Read()
        {
            throw new NotImplementedException();
        }

        public void ReadAll()
        {
            throw new NotImplementedException();
        }

        public static void Start()
        {
            if (securePrivateKey == null)
            {
                securePrivateKey = InputPrivateKey();
                securePrivateKey.MakeReadOnly();
            }
        }

        public static void Stop()
        {
            if (securePrivateKey != null)
            {
                securePrivateKey.Dispose();
            }
        }

        private static SecureString InputPrivateKey()
        {
            SecureString privateKey = new SecureString();
            ConsoleKeyInfo keyInfo;

            Console.Write("Enter the private key: ");

            do
            {
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Backspace && privateKey.Length > 0)
                {
                    privateKey.RemoveAt(privateKey.Length - 1);
                    Console.Write("\b \b");
                }
                else if (char.IsLetterOrDigit(keyInfo.KeyChar))
                {
                    privateKey.AppendChar(keyInfo.KeyChar);
                    Console.Write("*");
                }
            } while (keyInfo.Key != ConsoleKey.Enter);

            Console.WriteLine();

            return privateKey;
        }
    }
}
