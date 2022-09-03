using System;
using StockBox.Access.Users.Permissions.Helpers;

namespace StockBox.Access.Users.Permissions
{
    public interface ISbPermission
    {
        public Guid Token { get; }
        public string Name { get; }
        public ESbPermissionAccess Access { get; }
    }
}
