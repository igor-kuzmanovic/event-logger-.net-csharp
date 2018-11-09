using Helpers;
using System.Collections.Generic;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using WCFServiceCommon;

namespace WCFService
{
    internal class WCFService : IWCFService
    {
        public static SecureString PrivateKey { get; set; }

        public byte[] CheckIn()
        {
            X509Certificate2 certificate = SecurityHelper.GetUserCertificate(OperationContext.Current);

            EventLogger.AuthenticationSuccess(SecurityHelper.GetUsername(certificate));

            return RSAEncrypter.Encrypt(StringConverter.ToString(PrivateKey), certificate);
        }

        public void Add(string entry)
        {
            X509Certificate2 certificate = SecurityHelper.GetUserCertificate(OperationContext.Current);

            if (!RoleBasedAccessControl.UserHasPermission(certificate, Permissions.Add))
            {
                EventLogger.AuthorizationFailure(SecurityHelper.GetUsername(certificate), "Add", Permissions.Add.ToString());

                throw new FaultException("Unauthorized");
            }

            EventLogger.AuthorizationSuccess(SecurityHelper.GetUsername(certificate), "Add");
        }

        public void Update(int entryId, string entry)
        {
            X509Certificate2 certificate = SecurityHelper.GetUserCertificate(OperationContext.Current);

            if (!RoleBasedAccessControl.UserHasPermission(certificate, Permissions.Update))
            {
                EventLogger.AuthorizationFailure(SecurityHelper.GetUsername(certificate), "Update", Permissions.Update.ToString());
                EventLogger.Alarm(entryId);

                throw new FaultException("Unauthorized");
            }

            EventLogger.AuthorizationSuccess(SecurityHelper.GetUsername(certificate), "Update");
        }

        public void Delete(int entryId)
        {
            X509Certificate2 certificate = SecurityHelper.GetUserCertificate(OperationContext.Current);

            if (!RoleBasedAccessControl.UserHasPermission(certificate, Permissions.Delete))
            {
                EventLogger.AuthorizationFailure(SecurityHelper.GetUsername(certificate), "Delete", Permissions.Delete.ToString());
                EventLogger.Alarm(entryId);

                throw new FaultException("Unauthorized");
            }

            EventLogger.AuthorizationSuccess(SecurityHelper.GetUsername(certificate), "Delete");
        }

        public object Read(int entryId, byte[] key)
        {
            X509Certificate2 certificate = SecurityHelper.GetUserCertificate(OperationContext.Current);

            if (StringConverter.ToString(key) != StringConverter.ToString(PrivateKey))
            {
                EventLogger.AuthorizationFailure(SecurityHelper.GetUsername(certificate), "Read");

                throw new FaultException("Unauthorized");
            }

            EventLogger.AuthorizationSuccess(SecurityHelper.GetUsername(certificate), "Read");

            return new object();
        }

        public HashSet<object> ReadAll(byte[] key)
        {
            X509Certificate2 certificate = SecurityHelper.GetUserCertificate(OperationContext.Current);

            if (StringConverter.ToString(key) != StringConverter.ToString(PrivateKey))
            {
                EventLogger.AuthorizationFailure(SecurityHelper.GetUsername(certificate), "ReadAll");

                throw new FaultException("Unauthorized");
            }

            EventLogger.AuthorizationSuccess(SecurityHelper.GetUsername(certificate), "ReadAll");

            return new HashSet<object>();
        }
    }
}
