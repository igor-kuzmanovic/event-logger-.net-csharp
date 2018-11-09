﻿using System;
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
        public static X509Certificate2 GetCertificate(ChannelFactory factory)
        {
            return factory.Credentials.ClientCertificate.Certificate;
        }

        public static X509Certificate2 GetUserCertificate(OperationContext context)
        {
            return ((X509CertificateClaimSet)context.ServiceSecurityContext.AuthorizationContext.ClaimSets[0]).X509Certificate;
        }

        public static string GetUsername(X509Certificate2 certificate)
        {
            string username = string.Empty;

            string[] subjectNames = certificate.SubjectName.Name.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (subjectNames.Any(s => s.Contains("CN=")))
            {
                username = subjectNames.First(s => s.StartsWith("CN=")).Substring("CN=".Length).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[0];
            }

            return username;
        }

        public static HashSet<string> GetOrganizationalUnits(X509Certificate2 certificate)
        {
            HashSet<string> OUs = new HashSet<string>();

            string[] subjectName = certificate.SubjectName.Name.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (subjectName.Any(s => s.Contains("OU=")))
            {
                string[] OUArray = subjectName.First(s => s.StartsWith("OU=")).Substring("OU=".Length).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string OU in OUArray)
                {
                    OUs.Add(OU);
                }
            }

            return OUs;
        }
    }
}
