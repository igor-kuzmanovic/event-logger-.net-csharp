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
    }
}
