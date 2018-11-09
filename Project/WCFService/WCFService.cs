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

            EventLogger.AuthenticationSuccess(SecurityHelper.GetName(certificate));

            return RSAEncrypter.Encrypt(StringConverter.ToString(PrivateKey), certificate);
        }

        public void Add(string entry)
        {
            X509Certificate2 certificate = SecurityHelper.GetUserCertificate(OperationContext.Current);

            if (!RoleBasedAccessControl.UserHasPermission(certificate, Permissions.Add))
            {
                EventLogger.AuthorizationFailure(SecurityHelper.GetName(certificate), "Add", Permissions.Add.ToString());

                throw new FaultException("Unauthorized");
            }

            EventLogger.AuthorizationSuccess(SecurityHelper.GetName(certificate), "Add");
        }

        public void Update(int entryId, string entry)
        {
            X509Certificate2 certificate = SecurityHelper.GetUserCertificate(OperationContext.Current);

            if (!RoleBasedAccessControl.UserHasPermission(certificate, Permissions.Update))
            {
                EventLogger.AuthorizationFailure(SecurityHelper.GetName(certificate), "Update", Permissions.Update.ToString());
                EventLogger.Alarm(entryId);

                throw new FaultException("Unauthorized");
            }

            EventLogger.AuthorizationSuccess(SecurityHelper.GetName(certificate), "Update");
        }

        public void Delete(int entryId)
        {
            X509Certificate2 certificate = SecurityHelper.GetUserCertificate(OperationContext.Current);

            if (!RoleBasedAccessControl.UserHasPermission(certificate, Permissions.Delete))
            {
                EventLogger.AuthorizationFailure(SecurityHelper.GetName(certificate), "Delete", Permissions.Delete.ToString());
                EventLogger.Alarm(entryId);

                throw new FaultException("Unauthorized");
            }

            EventLogger.AuthorizationSuccess(SecurityHelper.GetName(certificate), "Delete");
        }

        public object Read(int entryId, byte[] key)
        {
            X509Certificate2 certificate = SecurityHelper.GetUserCertificate(OperationContext.Current);

            if (StringConverter.ToString(key) != StringConverter.ToString(PrivateKey))
            {
                EventLogger.AuthorizationFailure(SecurityHelper.GetName(certificate), "Read");

                throw new FaultException("Unauthorized");
            }

            EventLogger.AuthorizationSuccess(SecurityHelper.GetName(certificate), "Read");

            return new object();
        }

        public HashSet<object> ReadAll(byte[] key)
        {
            X509Certificate2 certificate = SecurityHelper.GetUserCertificate(OperationContext.Current);

            if (StringConverter.ToString(key) != StringConverter.ToString(PrivateKey))
            {
                EventLogger.AuthorizationFailure(SecurityHelper.GetName(certificate), "ReadAll");

                throw new FaultException("Unauthorized");
            }

            EventLogger.AuthorizationSuccess(SecurityHelper.GetName(certificate), "ReadAll");

            return new HashSet<object>();
        }
    }
}
