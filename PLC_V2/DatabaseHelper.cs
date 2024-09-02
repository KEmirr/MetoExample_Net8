using PLC_V2;
using System.Data.SqlClient;
using System.Data;

namespace PLC_V2
{


    public class DatabaseHelper
    {
        //private readonly string _connectionString;
        //private readonly IConfiguration _configuration;

        //public DatabaseHelper(IConfiguration configuration)
        //{
        //    _connectionString = configuration.GetConnectionString("DefaultConnection");
        //}
        //public async Task ExecuteStoredProcedureAsync(string procedureName, SqlParameter[] parameters)
        //{
        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();
        //        using (SqlCommand command = new SqlCommand(procedureName, connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.AddRange(parameters);
        //            await command.ExecuteNonQueryAsync();
        //        }
        //    }
        //}
        //public async Task<DataTable> ExecuteStoredProcedureWithResultAsync(string procedureName, SqlParameter[] parameters)
        //{
        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();
        //        using (SqlCommand command = new SqlCommand(procedureName, connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.AddRange(parameters);
        //            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
        //            {
        //                DataTable result = new DataTable();
        //                await Task.Run(() => adapter.Fill(result));
        //                return result;
        //            }
        //        }
        //    }
        //}
        //public async Task<Dictionary<string, string>> GetPLCSettingAsync()
        //{
        //    var setting = new Dictionary<string, string>();
        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();

        //        string query = "Select Aciklama,Deger FROM [dbo].[PLCSettings]";
        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {

        //            using (SqlDataReader reader = await command.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    setting[reader["Aciklama"].ToString()] = reader["Deger"].ToString();
        //                }
        //            }
        //        }
        //    }
        //    return setting;
        //}

        private readonly string _connectionString;

        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }
        private SqlConnection OpenConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public List<T> ExecuteStoredProcedure<T>(string spName, Func<SqlDataReader, T> mapFunction, Dictionary<string, object> parameters = null)
        {
            var results = new List<T>();

            using (var connection = OpenConnection())
            {
                using (var command = new SqlCommand(spName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(mapFunction(reader));
                        }
                    }
                }
            }

            return results;
        }
        public int ExecuteStoredProcedureNonQuery(string spName, Dictionary<string, object> parameters)
        {
            using (var connection = OpenConnection())
            {
                using (var command = new SqlCommand(spName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }

                    return command.ExecuteNonQuery();
                }
            }
        }
        public T ExecuteStoredProcedureScalar<T>(string spName, Dictionary<string, object> parameters)
        {
            using (var connection = OpenConnection())
            {
                using (var command = new SqlCommand(spName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }

                    return (T)command.ExecuteScalar();
                }
            }
        }
    }
}
