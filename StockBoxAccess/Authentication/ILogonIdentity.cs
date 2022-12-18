using System;
namespace StockBox.Access.Authentication
{
    public interface ILogonIdentity
    {
        string Username { get; }
        string Password { get; }
    }
}

