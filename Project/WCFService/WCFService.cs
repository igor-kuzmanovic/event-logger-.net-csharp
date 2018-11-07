using Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public byte[] CheckIn()
        {
            X509Certificate2 clientCertificate = GetUserCertificate();

            return RSAEncrypter.Encrypt(SecureStringConverter.ToString(securePrivateKey), clientCertificate);
        }

        public void Add()
        {
            X509Certificate2 clientCertificate = GetUserCertificate();

            if (!RoleBasedAccessControl.UserHasPermission(clientCertificate, Permissions.Add))
            {
                Console.WriteLine("[Add] Denied");
            }
        }

        public void Update()
        {
            X509Certificate2 clientCertificate = GetUserCertificate();

            if (!RoleBasedAccessControl.UserHasPermission(clientCertificate, Permissions.Update))
            {
                Console.WriteLine("[Update] Denied");
            }
        }

        public void Delete()
        {
            X509Certificate2 clientCertificate = GetUserCertificate();

            if (!RoleBasedAccessControl.UserHasPermission(clientCertificate, Permissions.Delete))
            {
                Console.WriteLine("[Delete] Denied");
            }

            return;
        }

        public void Read()
        {
            X509Certificate2 clientCertificate = GetUserCertificate();

            if (!RoleBasedAccessControl.UserHasPermission(clientCertificate, Permissions.Read))
            {
                Console.WriteLine("[Read] Denied");
            }
        }

        public void ReadAll()
        {
            X509Certificate2 clientCertificate = GetUserCertificate();

            if (!RoleBasedAccessControl.UserHasPermission(clientCertificate, Permissions.Read))
            {
                Console.WriteLine("[ReadAll] Denied");
            }
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

        private X509Certificate2 GetUserCertificate()
        {
            return ((X509CertificateClaimSet)OperationContext.Current.ServiceSecurityContext.AuthorizationContext.ClaimSets[0]).X509Certificate;
        }
    }
}
