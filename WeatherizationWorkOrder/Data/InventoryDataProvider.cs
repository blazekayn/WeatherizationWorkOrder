using Microsoft.AspNetCore.Http.Features;
using Microsoft.Data.SqlClient;
using System.Data;
using WeatherizationWorkOrder.Models;

namespace WeatherizationWorkOrder.Data
{
    public class InventoryDataProvider
    {
        private readonly IConfiguration _configuration;
        private string? _connectionString;
        public InventoryDataProvider(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration?.GetConnectionString("Weatherization");
        }

        public async Task Create(InventoryItem item)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = $"INSERT INTO [INVENTORY_ITEM] (Description, Cost, Units, StartingAmount, Remaining, PurchaseDate, LastModified, Created, CreatedBy, LastModifiedBy) " +
                             $"VALUES (@Description, @Cost, @Units, @StartingAmount, @Remaining, @PurchaseDate, @LastModified, @Created, @CreatedBy, @LastModifiedBy)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar).Value = item.Description;
                    cmd.Parameters.Add("@Cost", System.Data.SqlDbType.Decimal).Value = item.Cost;
                    cmd.Parameters.Add("@Units", System.Data.SqlDbType.NVarChar).Value = item.Units;
                    cmd.Parameters.Add("@StartingAmount", System.Data.SqlDbType.Decimal).Value = item.StartingAmount;
                    cmd.Parameters.Add("@Remaining", System.Data.SqlDbType.Decimal).Value = item.Remaining;
                    cmd.Parameters.Add("@PurchaseDate", System.Data.SqlDbType.DateTime).Value = item.PurchaseDate;
                    cmd.Parameters.Add("@LastModified", System.Data.SqlDbType.DateTime).Value = item.LastModified;
                    cmd.Parameters.Add("@Created", System.Data.SqlDbType.DateTime).Value = item.Created;
                    cmd.Parameters.Add("@CreatedBy", System.Data.SqlDbType.NVarChar).Value = item.CreatedBy;
                    cmd.Parameters.Add("@LastModifiedBy", System.Data.SqlDbType.NVarChar).Value = item.LastModifiedBy;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<InventoryItem> Read(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = $"SELECT * FROM INVENTORY_ITEM WHERE Id=@Id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr != null && dr.HasRows)
                        {
                            await dr.ReadAsync();
                            InventoryItem item = new InventoryItem
                            {
                                Id = dr.GetInt32("Id"),
                                Description = dr.GetString("Description"),
                                Cost = dr.GetDecimal("Cost"),
                                Units = dr.GetString("Units"),
                                StartingAmount = dr.GetDecimal("StartingAmount"),
                                Remaining = dr.GetDecimal("Remaining"),
                                PurchaseDate = dr.GetDateTime("PurchaseDate"),
                                LastModified = dr.GetDateTime("LastModified"),
                                Created = dr.GetDateTime("Created"),
                                CreatedBy = dr.GetString("CreatedBy"),
                                LastModifiedBy = dr.GetString("LastModifiedBy"),
                            };
                            return item;
                        }
                    }
                }
            }
            return default;
        }

        public async Task<List<InventoryItem>> ReadByDesc(string desc, string units)
        {
            List<InventoryItem> items = new List<InventoryItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = $"SELECT * FROM INVENTORY_ITEM WHERE UPPER(Description) = UPPER(@Description) AND UPPER(Units) = UPPER(@Units)";
                sql += "AND Remaining > 0 ";
                sql += "ORDER BY PurchaseDate desc";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar).Value = desc;
                    cmd.Parameters.Add("@Units", System.Data.SqlDbType.NVarChar).Value = units;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (await dr.ReadAsync())
                        {
                            InventoryItem item = new InventoryItem
                            {
                                Id = dr.GetInt32("Id"),
                                Description = dr.GetString("Description"),
                                Cost = dr.GetDecimal("Cost"),
                                Units = dr.GetString("Units"),
                                StartingAmount = dr.GetDecimal("StartingAmount"),
                                Remaining = dr.GetDecimal("Remaining"),
                                PurchaseDate = dr.GetDateTime("PurchaseDate"),
                                LastModified = dr.GetDateTime("LastModified"),
                                Created = dr.GetDateTime("Created"),
                                CreatedBy = dr.GetString("CreatedBy"),
                                LastModifiedBy = dr.GetString("LastModifiedBy")
                            };
                            items.Add(item);
                        }
                    }
                }
            }
            return items;
        }

        public async Task<List<InventoryItem>> Read(bool showOutOfStock, bool unique)
        {
            List<InventoryItem> items = new List<InventoryItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "";
                if (unique)
                {
                    sql = "SELECT DISTINCT description, units FROM INVENTORY_ITEM ";
                }
                else
                {
                    sql = $"SELECT * FROM INVENTORY_ITEM ";
                }
                if (!showOutOfStock)
                {
                    sql += " WHERE Remaining > 0 ";
                }
                if (!unique)
                {
                    sql += "ORDER BY PurchaseDate desc";
                }
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (await dr.ReadAsync())
                        {
                            if (unique)
                            {
                                InventoryItem item = new InventoryItem
                                {
                                    Description = dr.GetString("Description"),
                                    Units = dr.GetString("Units"),
                                };
                                items.Add(item);
                            }
                            else
                            {
                                InventoryItem item = new InventoryItem
                                {
                                    Id = dr.GetInt32("Id"),
                                    Description = dr.GetString("Description"),
                                    Cost = dr.GetDecimal("Cost"),
                                    Units = dr.GetString("Units"),
                                    StartingAmount = dr.GetDecimal("StartingAmount"),
                                    Remaining = dr.GetDecimal("Remaining"),
                                    PurchaseDate = dr.GetDateTime("PurchaseDate"),
                                    LastModified = dr.GetDateTime("LastModified"),
                                    Created = dr.GetDateTime("Created"),
                                    CreatedBy = dr.GetString("CreatedBy"),
                                    LastModifiedBy = dr.GetString("LastModifiedBy")
                                };
                                items.Add(item);
                            }
                        }
                    }
                }
            }
            return items;
        }

        public async Task Update(InventoryItem item)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = $"UPDATE [INVENTORY_ITEM] SET " +
                             $"Description=@Description, " +
                             $"Cost=@Cost, " +
                             $"Units=@Units, " +
                             $"StartingAmount=@StartingAmount, " +
                             $"Remaining=@Remaining, " +
                             $"PurchaseDate=@PurchaseDate, " +
                             $"LastModified=@LastModified, " +
                             $"Created=@Created, " +
                             $"CreatedBy=@CreatedBy, " +
                             $"LastModifiedBy=@LastModifiedBy " +
                             $"WHERE Id=@Id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar).Value = item.Description;
                    cmd.Parameters.Add("@Cost", System.Data.SqlDbType.Decimal).Value = item.Cost;
                    cmd.Parameters.Add("@Units", System.Data.SqlDbType.NVarChar).Value = item.Units;
                    cmd.Parameters.Add("@StartingAmount", System.Data.SqlDbType.Decimal).Value = item.StartingAmount;
                    cmd.Parameters.Add("@Remaining", System.Data.SqlDbType.Decimal).Value = item.Remaining;
                    cmd.Parameters.Add("@PurchaseDate", System.Data.SqlDbType.DateTime).Value = item.PurchaseDate;
                    cmd.Parameters.Add("@LastModified", System.Data.SqlDbType.DateTime).Value = item.LastModified;
                    cmd.Parameters.Add("@Created", System.Data.SqlDbType.DateTime).Value = item.Created;
                    cmd.Parameters.Add("@CreatedBy", System.Data.SqlDbType.NVarChar).Value = item.CreatedBy;
                    cmd.Parameters.Add("@LastModifiedBy", System.Data.SqlDbType.NVarChar).Value = item.LastModifiedBy;
                    cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = item.Id;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = $"DELETE FROM [INVENTORY_ITEM] WHERE Id=@Id ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<string>> ReadUnits()
        {
            List<string> units = new List<string>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = $"SELECT DISTINCT units FROM INVENTORY_ITEM ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (await dr.ReadAsync())
                        {
                            units.Add(dr.GetString("Units"));
                        }
                    }
                }
            }
            return units;
        }
    }
}
