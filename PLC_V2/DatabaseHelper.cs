using PLC_V2;
using System.Data.SqlClient;
using System.Data;

namespace PLC_V2
{
    //EmirhanKARAKURT
    
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper (string connectionString)
        {
            _connectionString=connectionString;
        }
        public async Task ExecuteStoredProcedureAsync(string procedureName, SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task<DataTable> ExecuteStoredProcedureWithResultAsync(string procedureName, SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable result = new DataTable();
                        await Task.Run(() => adapter.Fill(result));
                        return result;
                    }
                }
            }
        }
        public async Task<Dictionary<string, string>> GetPLCSettingAsync()
        {
            var setting = new Dictionary<string, string>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "Select Aciklama,Deger FROM [dbo].[PLCSettings]";
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            setting[reader["Aciklama"].ToString()] = reader["Deger"].ToString();
                        }
                    }
                }
            }
            return setting;
        }

    }
}
