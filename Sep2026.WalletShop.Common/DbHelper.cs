using Microsoft.Data.SqlClient;

namespace Sep2026.WalletShop
{
    public static class DbHelper
    {
        private static readonly string _connectionString = BuildConnectionString();

        private static string BuildConnectionString()
        {
            return new SqlConnectionStringBuilder
            {
                DataSource            = ".",
                InitialCatalog        = "Sep2026Db",
                UserID                = "sa",
                Password              = "sasa@123",
                TrustServerCertificate = true
            }.ConnectionString;
        }

        public static SqlConnection GetOpenConnection()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
