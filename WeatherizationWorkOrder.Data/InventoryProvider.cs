using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WeatherizationWorkOrder.Data.Models;

namespace WeatherizationWorkOrder.Data
{
    public class InventoryProvider : InventoryProviderInterface
    {
        private readonly IConfiguration _configuration;
        private string? _connectionString;
        public InventoryProvider(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration?.GetConnectionString("NATIONS");
        }

        public async Task CreateInventoryItem(InventoryItem item)
        {
            throw new NotImplementedException();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = $"INSERT INTO [User] (Id, Username, Password, Salt, DisplayName) VALUES (@Id, @Username, @Password, @Salt, @DisplayName)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //cmd.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier).Value = item.Id;
                    //cmd.Parameters.Add("@Username", System.Data.SqlDbType.NVarChar).Value = item.;
                    //cmd.Parameters.Add("@Password", System.Data.SqlDbType.NVarChar).Value = password;
                    //cmd.Parameters.Add("@Salt", System.Data.SqlDbType.NVarChar).Value = salt;
                    //cmd.Parameters.Add("@DisplayName", System.Data.SqlDbType.NVarChar).Value = displayName;
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteInventoryItem(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<InventoryItem>> GetAllInventoryItems()
        {
            throw new NotImplementedException();
        }

        public async Task<InventoryItem> GetInventoryItemById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateInventoryItem(InventoryItem item)
        {
            throw new NotImplementedException();
        }
    }
    public interface InventoryProviderInterface
    {
        Task CreateInventoryItem(InventoryItem item);
        Task UpdateInventoryItem(InventoryItem item);
        Task<InventoryItem> GetInventoryItemById(int id);
        Task<List<InventoryItem>> GetAllInventoryItems();
        Task DeleteInventoryItem(int id);
    }
}