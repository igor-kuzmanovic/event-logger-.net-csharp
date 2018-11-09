using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace WCFService
{
    internal static class RoleBasedAccessControl
    {
        private static readonly Dictionary<Roles, HashSet<Permissions>> rolePermissions;

        static RoleBasedAccessControl()
        {
            rolePermissions = new Dictionary<Roles, HashSet<Permissions>>
            {
                {
                    Roles.Client,
                    new HashSet<Permissions>()
                    {
                        Permissions.Add
                    }
                },
                {
                    Roles.Moderator,
                    new HashSet<Permissions>()
                    {
                        Permissions.Update
                    }
                },
                {
                    Roles.Administrator,
                    new HashSet<Permissions>()
                    {
                        Permissions.Delete
                    }
                }
            };
        }

        public static bool UserHasPermission(X509Certificate2 user, Permissions permission)
        {
            bool result = false;

            HashSet<Roles> roles = GetUserRoles(user);
            result = roles.Any(role => RoleHasPermission(role, permission));

            return result;
        }

        private static HashSet<Roles> GetUserRoles(X509Certificate2 user)
        {
            HashSet<Roles> roles = new HashSet<Roles>();

            HashSet<string> organizationalUnits = SecurityHelper.GetOrganizationalUnits(user);

            foreach (string organizationalUnit in organizationalUnits)
            {
                if (Enum.TryParse(organizationalUnit, out Roles role))
                {
                    roles.Add(role);
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
