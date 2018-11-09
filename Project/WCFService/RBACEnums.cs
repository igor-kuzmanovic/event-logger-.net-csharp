using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFService
{
    internal enum Roles
    {
        Client,
        Moderator,
        Administrator
    }

    internal enum Permissions
    {
        Add,
        Update,
        Delete
    }
}
