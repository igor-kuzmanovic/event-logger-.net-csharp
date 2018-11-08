using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WCFService
{
    internal static class RoleBasedAccessControl
    {
        private static readonly Dictionary<Roles, HashSet<Permissions>> rolePermissions;

        static RoleBasedAccessControl()
        {
            rolePermissions = new Dictionary<Roles, HashSet<Permissions>>();

            rolePermissions.Add(Roles.Client, new HashSet<Permissions>()
            {
                Permissions.Read,
                Permissions.Add
            });

            rolePermissions.Add(Roles.Moderator, new HashSet<Permissions>()
            {
                Permissions.Read,
                Permissions.Update
            });

            rolePermissions.Add(Roles.Administrator, new HashSet<Permissions>()
            {
                Permissions.Read,
                //Permissions.Delete
            });
        }

        public static bool UserHasPermission(X509Certificate2 user, Permissions permission)
        {
            HashSet<Roles> roles = GetUserRoles(user);

            foreach (Roles role in roles)
            {
                if (RoleHasPermission(role, permission))
                {
                    return true;
                }
            }

            return false;
        }

        private static HashSet<Roles> GetUserRoles(X509Certificate2 user)
        {
            HashSet<Roles> roles = new HashSet<Roles>();

            string[] subjectName = user.SubjectName.Name.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (subjectName.Any(s => s.Contains("OU=")))
            {
                string[] organizationalUnits = subjectName.First(s => s.StartsWith("OU=")).Substring("OU=".Length).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string organizationalUnit in organizationalUnits)
                {
                    if (Enum.TryParse(organizationalUnit, out Roles role))
                    {
                        roles.Add(role);
                    }
                }
            }

            return roles;
        }

        private static bool RoleHasPermission(Roles role, Permissions permission)
        {
            return rolePermissions.ContainsKey(role) && rolePermissions[role].Contains(permission);
        }
    }
}
