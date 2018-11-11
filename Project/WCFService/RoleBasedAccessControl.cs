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
                    // Give the Client role a permission to Add
                    Roles.Client,
                    new HashSet<Permissions>()
                    {
                        Permissions.Add
                    }
                },
                {
                    // Give the Moderator role a permission to Update
                    Roles.Moderator,
                    new HashSet<Permissions>()
                    {
                        Permissions.Update
                    }
                },
                {
                    // Give the Administrator role a permission to Delete
                    Roles.Administrator,
                    new HashSet<Permissions>()
                    {
                        Permissions.Delete
                    }
                }
            };
        }

        private static HashSet<Roles> GetUserRoles(X509Certificate2 user)
        {
            HashSet<Roles> roles = new HashSet<Roles>();

            // Get all the organizational units for the specified user
            HashSet<string> organizationalUnits = SecurityHelper.GetOrganizationalUnits(user);

            foreach (string organizationalUnit in organizationalUnits)
            {
                // Try to parse the organizational units into roles
                if (Enum.TryParse(organizationalUnit, out Roles role))
                {
                    // Add the parsed role into the set
                    roles.Add(role);
                }
            }

            return roles;
        }

        private static bool RoleHasPermission(Roles role, Permissions permission)
        {
            bool hasPermission = false;

            // Check if the specified role contains the required permission
            hasPermission = rolePermissions[role].Contains(permission);

            return hasPermission;
        }

        public static bool UserHasPermission(X509Certificate2 user, Permissions permission)
        {
            bool hasPermission = false;

            // Get all the roles for the specified user
            HashSet<Roles> userRoles = GetUserRoles(user);

            // Check if any of those roles has the required permission
            hasPermission = userRoles.Any(role => RoleHasPermission(role, permission));

            return hasPermission;
        }
    }
}
