using System;
using StockBox.Access.Users.Permissions;
using StockBox.Validation;

namespace StockBox.Access.Users
{

    public interface ISbUser
    {
        public Guid Token { get; }
        public int SbUserId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }

        public string GetFormattedUserName();

        public ValidationResultList HasAccess(ISbPermission tryAccess);
    }
}
