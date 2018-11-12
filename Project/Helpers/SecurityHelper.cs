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
            X509Certificate2 certificate = new X509Certificate2();

            // Get the certificate from the channel factory (a proxy for a service)
            certificate = factory.Credentials.ClientCertificate.Certificate;

            return certificate;
        }

        public static X509Certificate2 GetCertificate(OperationContext context)
        {
            X509Certificate2 certificate = new X509Certificate2();

            // Get the certificate from the security context of the operation context (a client calling a service method)
            certificate = (context.ServiceSecurityContext.AuthorizationContext.ClaimSets[0] as X509CertificateClaimSet).X509Certificate;

            return certificate;
        }

        public static string GetName(X509Certificate2 certificate)
        {
            string name = string.Empty;

            // Get the client's subject name ('CN=Name, OU=OrgUnit1|OrgUnit2...')
            string subjectName = certificate.SubjectName.Name;

            // Split the subject name attributes into separate strings ('CN=Name', 'OU=OrgUnit1|OrgUnit2...')
            string[] subjectAttributes = subjectName.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (subjectAttributes.Any(s => s.Contains("CN=")))
            {
                // Get the common name attribute ('CN=Name')
                string commonName = subjectAttributes.First(s => s.StartsWith("CN="));

                // Remove the 'CN=' from the common name attribute ('Name')
                name = commonName.Substring("CN=".Length);
            }

            return name;
        }

        public static string GetName(OperationContext context)
        {
            string name = string.Empty;

            // Get the windows identity from the security context of the operation context (a client calling a service method)
            WindowsIdentity identity = context.ServiceSecurityContext.WindowsIdentity;

            // Get the name from the windows identity
            name = GetName(identity);

            return name;
        }

        public static string GetName(WindowsIdentity identity)
        {
            string name = string.Empty;

            // Get the name from the windows identity and parse it to remove the domain name
            name = ParseName(identity.Name);

            return name;
        }

        public static HashSet<string> GetOrganizationalUnits(X509Certificate2 certificate)
        {
            HashSet<string> orgUnitSet = new HashSet<string>();

            // Get the client's subject name ('CN=Name, OU=OrgUnit1|OrgUnit2...')
            string subjectName = certificate.SubjectName.Name;

            // Split the subject name attributes into separate strings ('CN=Name', 'OU=OrgUnit1|OrgUnit2...')
            string[] subjectAttributes = subjectName.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (subjectAttributes.Any(s => s.Contains("OU=")))
            {
                // Get the organizational units attribute ('OU=OrgUnit1|OrgUnit2...')
                string orgUnitsAttribute = subjectAttributes.First(s => s.StartsWith("OU="));

                // Remove the 'OU=' from the organizational units attribute ('OrgUnit1|OrgUnit2...')
                string orgUnitsValues = orgUnitsAttribute.Substring("OU=".Length);

                // Split the organizational unit values into separate strings ('OrgUnit1', 'OrgUnit2', ...)
                string[] orgUnits = orgUnitsValues.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string orgUnit in orgUnits)
                {
                    // Add all organizational units into the set
                    orgUnitSet.Add(orgUnit);
                }
            }

            return orgUnitSet;
        }

        public static string ParseName(string windowsName)
        {
            string name = string.Empty;

            if (windowsName.Contains("@"))
            {
                // Get the name from the name@domain format
                name = windowsName.Split('@')[0];
            }
            else if (windowsName.Contains("\\"))
            {
                // Get the name from the domain\name format
                name = windowsName.Split('\\')[1];
            }
            else
            {
                // The name is already parsed
                name = windowsName;
            }

            return name;
        }
    }
}
