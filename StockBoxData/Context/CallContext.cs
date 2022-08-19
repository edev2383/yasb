using Npgsql;
using StockBox.Associations;


namespace StockBox.Data.Context
{

    public class CallContext : ICallContextProvider
    {

        private string _connectionString;
        private NpgsqlConnection _conn;

        public CallContext(string connectionString)
        {
            //_connectionString = connectionString;
            //_conn = new NpgsqlConnection(_connectionString);
            //_conn.Open();

            //var sql = "SELECT version()";

            //using var cmd = new NpgsqlCommand(sql, _conn);

            //var version = cmd.ExecuteScalar().ToString();
        }
    }
}
