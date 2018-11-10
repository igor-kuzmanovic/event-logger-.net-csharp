using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;

namespace Helpers
{
    public static class SecurityHelper
    {
        public static X509Certificate2 GetCertificate(ChannelFactory factory)
        {
            return factory.Credentials.ClientCertificate.Certificate;
        }

        public static X509Certificate2 GetUserCertificate(OperationContext context)
        {
            return (context.ServiceSecurityContext.AuthorizationContext.ClaimSets[0] as X509CertificateClaimSet).X509Certificate;
        }

        public static string GetName(X509Certificate2 certificate)
        {
            string name = string.Empty;

            string[] subjectNames = certificate.SubjectName.Name.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (subjectNames.Any(s => s.Contains("CN=")))
            {
                name = subjectNames.First(s => s.StartsWith("CN=")).Substring("CN=".Length).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[0];
            }

            return name;
        }

        public static string GetName(OperationContext context)
        {
            string name = string.Empty;

            name = ParseName(context.ServiceSecurityContext.WindowsIdentity.Name);

            return name;
        }

        public static string GetName(WindowsIdentity identity)
        {
            string name = string.Empty;

            name = ParseName(identity.Name);

            return name;
        }

        public static HashSet<string> GetOrganizationalUnits(X509Certificate2 certificate)
        {
            HashSet<string> organizationalUnitSet = new HashSet<string>();

            string[] subjectNames = certificate.SubjectName.Name.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (subjectNames.Any(s => s.Contains("OU=")))
            {
                string[] organizationalUnits = subjectNames.First(s => s.StartsWith("OU=")).Substring("OU=".Length).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string organizationalUnit in organizationalUnits)
                {
                    organizationalUnitSet.Add(organizationalUnit);
                }
            }

            return organizationalUnitSet;
        }

        public static string ParseName(string logonName)
        {
            string name = string.Empty;

            if (logonName.Contains("@"))
            {
                name = logonName.Split('@')[0];
            }
            else if (logonName.Contains("\\"))
            {
                name = logonName.Split('\\')[1];
            }
            else
            {
                name = logonName;
            }

            return name;
        }
    }
}
