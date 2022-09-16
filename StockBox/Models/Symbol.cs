using System;
using StockBox.Associations;

namespace StockBox.Models
{

    /// <summary>
    /// 
    /// </summary>
    public class Symbol : ISymbolProvider
    {

        public Guid? Token { get; set; }
        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }

        public Symbol(string name, Guid? token = null, DateTime? createDate = null, string createUser = null)
        {
            Name = name;
            Token = token;
            CreateDate = createDate;
            CreateUser = createUser;
        }
    }
}
