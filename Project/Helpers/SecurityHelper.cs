using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class SecurityHelper
    {
        public static X509Certificate2 GetUserCertificate(ChannelFactory client)
        {
            return client.Credentials.ClientCertificate.Certificate;
        }

        public static X509Certificate2 GetUserCertificate(OperationContext context)
        {
            return ((X509CertificateClaimSet)context.ServiceSecurityContext.AuthorizationContext.ClaimSets[0]).X509Certificate;
        }

        public static string GetUserUsername(X509Certificate2 certificate)
        {
            string subjectName = string.Empty;
            string[] subjectNames = certificate.SubjectName.Name.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (subjectNames.Any(s => s.Contains("CN=")))
            {
                subjectName = subjectNames.First(s => s.StartsWith("CN=")).Substring("CN=".Length).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[0];
            }

            return subjectName;
        }
    }
}
