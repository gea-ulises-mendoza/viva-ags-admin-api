
using System.Configuration;
using System.Data.SqlClient;
using VivaAguascalientesAPI;

namespace VivaAguascalientesAPI.DAO
{
    public class DbContext
    {
        private static string _connectionString = string.Empty;

        private static int _commandTimeout = 30;

        public static string GetConnectionString()
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                _connectionString = AppSettingsProvider.DbConnectionString;
            }
            //_connectionString = "Data Source=10.1.111.58\\SQLSAEDB;Initial Catalog=BD_Moviles;User ID=usuMoviles;Password=u5u%67MoV; Connect Timeout=1800";
            //_connectionString = "Data Source=10.1.111.173;Initial Catalog=BD_Moviles;User ID=usuMoviles_Des;Password=MOV%u5ud; Connect Timeout=1800";
            return _connectionString;
        }

        public static int CommandTimeout
        {
            get
            {
                try
                {
                    return int.Parse(AppSettingsProvider.CommandTimeoutForDb);
                }
                catch
                {

                    return _commandTimeout;
                }
            }
        }

        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(GetConnectionString());
        }
    }
}
