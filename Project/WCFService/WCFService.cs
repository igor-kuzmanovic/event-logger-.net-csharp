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
        public static SecureString PrivateKey { get; set; }

        public byte[] CheckIn()
        {
            X509Certificate2 clientCertificate = SecurityHelper.GetUserCertificate(OperationContext.Current);

            return RSAEncrypter.Encrypt(SecureStringConverter.ToString(PrivateKey), clientCertificate);
        }

        public void Add()
        {
            X509Certificate2 clientCertificate = SecurityHelper.GetUserCertificate(OperationContext.Current);

            if (!RoleBasedAccessControl.UserHasPermission(clientCertificate, Permissions.Add))
            {
                Console.WriteLine("[Add] Denied");
            }
        }

        public void Update()
        {
            X509Certificate2 clientCertificate = SecurityHelper.GetUserCertificate(OperationContext.Current);

            if (!RoleBasedAccessControl.UserHasPermission(clientCertificate, Permissions.Update))
            {
                Console.WriteLine("[Update] Denied");
            }
        }

        public void Delete()
        {
            X509Certificate2 clientCertificate = SecurityHelper.GetUserCertificate(OperationContext.Current);

            if (!RoleBasedAccessControl.UserHasPermission(clientCertificate, Permissions.Delete))
            {
                Console.WriteLine("[Delete] Denied");
            }

            return;
        }

        public void Read()
        {
            X509Certificate2 clientCertificate = SecurityHelper.GetUserCertificate(OperationContext.Current);

            if (!RoleBasedAccessControl.UserHasPermission(clientCertificate, Permissions.Read))
            {
                Console.WriteLine("[Read] Denied");
            }
        }

        public void ReadAll()
        {
            X509Certificate2 clientCertificate = SecurityHelper.GetUserCertificate(OperationContext.Current);

            if (!RoleBasedAccessControl.UserHasPermission(clientCertificate, Permissions.Read))
            {
                Console.WriteLine("[ReadAll] Denied");
            }
        }
    }
}
