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

            EventLogger.AuthenticationSuccess(SecurityHelper.GetUserUsername(clientCertificate));

            return RSAEncrypter.Encrypt(StringConverter.ToString(PrivateKey), clientCertificate);
        }

        public void Add(string entry)
        {
            X509Certificate2 clientCertificate = SecurityHelper.GetUserCertificate(OperationContext.Current);

            if (!RoleBasedAccessControl.UserHasPermission(clientCertificate, Permissions.Add))
            {
                EventLogger.AuthorizationFailure(SecurityHelper.GetUserUsername(clientCertificate), "Add", Permissions.Add.ToString());

                throw new FaultException("Unauthorized");
            }

            EventLogger.AuthorizationSuccess(SecurityHelper.GetUserUsername(clientCertificate), "Add");
        }

        public void Update(int entryId, string entry)
        {
            X509Certificate2 clientCertificate = SecurityHelper.GetUserCertificate(OperationContext.Current);

            if (!RoleBasedAccessControl.UserHasPermission(clientCertificate, Permissions.Update))
            {
                EventLogger.AuthorizationFailure(SecurityHelper.GetUserUsername(clientCertificate), "Update", Permissions.Update.ToString());
                EventLogger.Alarm(entryId);

                throw new FaultException("Unauthorized");
            }

            EventLogger.AuthorizationSuccess(SecurityHelper.GetUserUsername(clientCertificate), "Update");
        }

        public void Delete(int entryId)
        {
            X509Certificate2 clientCertificate = SecurityHelper.GetUserCertificate(OperationContext.Current);

            if (!RoleBasedAccessControl.UserHasPermission(clientCertificate, Permissions.Delete))
            {
                EventLogger.AuthorizationFailure(SecurityHelper.GetUserUsername(clientCertificate), "Delete", Permissions.Delete.ToString());
                EventLogger.Alarm(entryId);

                throw new FaultException("Unauthorized");
            }

            EventLogger.AuthorizationSuccess(SecurityHelper.GetUserUsername(clientCertificate), "Delete");
        }

        public object Read(int entryId)
        {
            X509Certificate2 clientCertificate = SecurityHelper.GetUserCertificate(OperationContext.Current);

            if (!RoleBasedAccessControl.UserHasPermission(clientCertificate, Permissions.Read))
            {
                EventLogger.AuthorizationFailure(SecurityHelper.GetUserUsername(clientCertificate), "Read", Permissions.Read.ToString());

                throw new FaultException("Unauthorized");
            }

            EventLogger.AuthorizationSuccess(SecurityHelper.GetUserUsername(clientCertificate), "Read");

            return new object();
        }

        public HashSet<object> ReadAll()
        {
            X509Certificate2 clientCertificate = SecurityHelper.GetUserCertificate(OperationContext.Current);

            if (!RoleBasedAccessControl.UserHasPermission(clientCertificate, Permissions.Read))
            {
                EventLogger.AuthorizationFailure(SecurityHelper.GetUserUsername(clientCertificate), "ReadAll", Permissions.Read.ToString());

                throw new FaultException("Unauthorized");
            }

            EventLogger.AuthorizationSuccess(SecurityHelper.GetUserUsername(clientCertificate), "ReadAll");

            return new HashSet<object>();
        }
    }
}
