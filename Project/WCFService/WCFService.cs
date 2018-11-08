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

            return RSAEncrypter.Encrypt(StringConverter.ToString(PrivateKey), clientCertificate);
        }

        public void Add(string entry)
        {
            X509Certificate2 clientCertificate = SecurityHelper.GetUserCertificate(OperationContext.Current);

            if (!RoleBasedAccessControl.UserHasPermission(clientCertificate, Permissions.Add))
            {
                throw new FaultException("Unauthorized");
            }

            Console.WriteLine("Added a new entry");
        }

        public void Update(int entryId, string entry)
        {
            X509Certificate2 clientCertificate = SecurityHelper.GetUserCertificate(OperationContext.Current);

            if (!RoleBasedAccessControl.UserHasPermission(clientCertificate, Permissions.Update))
            {
                throw new FaultException("Unauthorized");
            }

            Console.WriteLine("Updated {0} entry", entryId);
        }

        public void Delete(int entryId)
        {
            X509Certificate2 clientCertificate = SecurityHelper.GetUserCertificate(OperationContext.Current);

            if (!RoleBasedAccessControl.UserHasPermission(clientCertificate, Permissions.Delete))
            {
                throw new FaultException("Unauthorized");
            }

            Console.WriteLine("Deleted {0} entry", entryId);
        }

        public KeyValuePair<int, string> Read(int entryId)
        {
            X509Certificate2 clientCertificate = SecurityHelper.GetUserCertificate(OperationContext.Current);

            if (!RoleBasedAccessControl.UserHasPermission(clientCertificate, Permissions.Read))
            {
                throw new FaultException("Unauthorized");
            }

            Console.WriteLine("Read {0} entry", entryId);

            return new KeyValuePair<int, string>(0, string.Empty);
        }

        public Dictionary<int, string> ReadAll()
        {
            X509Certificate2 clientCertificate = SecurityHelper.GetUserCertificate(OperationContext.Current);

            if (!RoleBasedAccessControl.UserHasPermission(clientCertificate, Permissions.Read))
            {
                throw new FaultException("Unauthorized");
            }

            Console.WriteLine("Read all entries");

            return new Dictionary<int, string>();
        }
    }
}
