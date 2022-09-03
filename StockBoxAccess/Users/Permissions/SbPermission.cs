using System;
using StockBox.Access.Users.Permissions.Helpers;


namespace StockBox.Access.Users.Permissions
{
    /// <summary>
    /// Class encapsulates an explicit user access model. A user object can
    /// be queried for access
    /// </summary>
    public class SbPermission : ISbPermission
    {

        public Guid Token { get { return _token; } }
        public string Name { get { return _name; } }
        public ESbPermissionAccess Access { get { return _access; } }
        public DateTime? Expiration { get; set; }

        private Guid _token;
        private string _name;
        private ESbPermissionAccess _access;

        public SbPermission(Guid token, string name, ESbPermissionAccess access)
        {
            _token = token;
            _name = name;
            _access = access;
        }

        public SbPermission() { }
    }
}
