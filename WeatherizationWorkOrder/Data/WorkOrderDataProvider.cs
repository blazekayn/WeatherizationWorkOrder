using Microsoft.AspNetCore.Http.Features;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using WeatherizationWorkOrder.Models;

namespace WeatherizationWorkOrder.Data
{
    public class WorkOrderDataProvider
    {
        private readonly IConfiguration _configuration;
        private string? _connectionString;
        public WorkOrderDataProvider(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration?.GetConnectionString("Weatherization");
        }

        public async Task Create(WorkOrder item)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "INSERT INTO[WORK_ORDER](Consumer, PreparedBy, Description, PreparedDate, LastModifiedBy, LastModified) " +
                             "VALUES('Aaron', 'Tina', 'oijpoij', '01/01/2024', 'Aaron', '06/19/2024')";
                //string sql = $"INSERT INTO [WORK_ORDER] (Consumer, PreparedBy, Description, PreparedDate, LastModifiedBy, LastModified) " +
                //             $"VALUES (@Consumer, @PreparedBy, @Description, @PreparedDate, @LastModifiedBy, @LastModified)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //cmd.Parameters.Add("@Consumer", System.Data.SqlDbType.NVarChar).Value = item.Consumer;
                    //cmd.Parameters.Add("@PreparedBy", System.Data.SqlDbType.NVarChar).Value = item.PreparedBy;
                    //cmd.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar).Value = item.Description;
                    //cmd.Parameters.Add("@PreparedDate", System.Data.SqlDbType.DateTime).Value = item.PreparedDate;
                    //cmd.Parameters.Add("@LastModifiedBy", System.Data.SqlDbType.NVarChar).Value = item.LastModifiedBy;
                    //cmd.Parameters.Add("@LastModified", System.Data.SqlDbType.DateTime).Value = item.LastModified;

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

        public async Task<List<InventoryItem>> Read(bool showOutOfStock)
        {
            List<InventoryItem> items = new List<InventoryItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = $"SELECT * FROM INVENTORY_ITEM ";
                if (!showOutOfStock)
                {
                    sql += " WHERE Remaining > 0 ";
                }
                    sql += "ORDER BY PurchaseDate desc";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
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
