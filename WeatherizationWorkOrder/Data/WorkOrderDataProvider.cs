using Azure.Core;
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

        public async Task<int> Create(WorkOrder item)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = $"INSERT INTO [WORK_ORDER] (Consumer, PreparedBy, Description, PreparedDate, LastModifiedBy, LastModified) " +
                             $"OUTPUT Inserted.ID " +
                             $"VALUES (@Consumer, @PreparedBy, @Description, @PreparedDate, @LastModifiedBy, @LastModified)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Consumer", System.Data.SqlDbType.NVarChar).Value = item.Consumer;
                    cmd.Parameters.Add("@PreparedBy", System.Data.SqlDbType.NVarChar).Value = item.PreparedBy;
                    cmd.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar).Value = item.Description;
                    cmd.Parameters.Add("@PreparedDate", System.Data.SqlDbType.DateTime).Value = item.PreparedDate;
                    cmd.Parameters.Add("@LastModifiedBy", System.Data.SqlDbType.NVarChar).Value = item.LastModifiedBy;
                    cmd.Parameters.Add("@LastModified", System.Data.SqlDbType.DateTime).Value = item.LastModified;

                    return (int)await cmd.ExecuteScalarAsync();
                }
            }
        }

        public async Task<WorkOrder> Read(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = $"SELECT * FROM WORK_ORDER WHERE Id=@Id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr != null && dr.HasRows)
                        {
                            await dr.ReadAsync();
                            WorkOrder workOrder = new WorkOrder
                            {
                                Id = dr.GetInt32("Id"),
                                Consumer = dr.GetString("Consumer"),
                                PreparedBy = dr.GetString("PreparedBy"),
                                Description = dr.GetString("Description"),
                                PreparedDate = dr.GetDateTime("PreparedDate"),
                                LastModifiedBy = dr.GetString("LastModifiedBy"),
                                LastModified = dr.GetDateTime("LastModified"),
                            };
                            return workOrder;
                        }
                    }
                }
            }
            return default;
        }

        public async Task<List<WorkOrder>> Read()
        {
            List<WorkOrder> items = new List<WorkOrder>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = $"SELECT * FROM WORK_ORDER ORDER BY PreparedDate DESC";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (await dr.ReadAsync())
                        {
                            WorkOrder workOrder = new WorkOrder
                            {
                                Id = dr.GetInt32("Id"),
                                Consumer = dr.GetString("Consumer"),
                                PreparedBy = dr.GetString("PreparedBy"),
                                Description = dr.GetString("Description"),
                                PreparedDate = dr.GetDateTime("PreparedDate"),
                                LastModifiedBy = dr.GetString("LastModifiedBy"),
                                LastModified = dr.GetDateTime("LastModified"),
                            };
                            items.Add(workOrder);
                        }
                    }
                }
            }
            return items;
        }

        public async Task<List<Material>> ReadMaterials(int id)
        {
            List<Material> items = new List<Material>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = $"SELECT WM.Id, I.Description, I.Cost, WM.UnitsUsed, I.Cost * WM.UnitsUsed As Total FROM WO_MATERIAL WM JOIN INVENTORY_ITEM I ON WM.InventoryItemId = I.Id WHERE WM.WorkOrderId=@WorkOrderId";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@WorkOrderId", System.Data.SqlDbType.Int).Value = id;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (await dr.ReadAsync())
                        {
                            Material item = new Material
                            {
                                Id = dr.GetInt32("Id"),
                                Description = dr.GetString("Description"),
                                CostPer = dr.GetDecimal("Cost"),
                                AmountUsed = dr.GetDecimal("UnitsUsed"),
                                Total = dr.GetDecimal("Total")

                            };
                            items.Add(item);
                        }
                    }
                }
            }
            return items;
        }

        public async Task Update(WorkOrder workOrder)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = $"UPDATE [WORK_ORDER] SET " +
                             $"Consumer=@Consumer, " +
                             $"PreparedBy=@PreparedBy, " +
                             $"Description=@Description, " +
                             $"LastModifiedBy=@LastModifiedBy, " +
                             $"LastModified=@LastModified " +
                             $"WHERE Id=@Id; ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = workOrder.Id;
                    cmd.Parameters.Add("@Consumer", System.Data.SqlDbType.NVarChar).Value = workOrder.Consumer;
                    cmd.Parameters.Add("@PreparedBy", System.Data.SqlDbType.NVarChar).Value = workOrder.PreparedBy;
                    cmd.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar).Value = workOrder.Description;
                    cmd.Parameters.Add("@LastModifiedBy", System.Data.SqlDbType.NVarChar).Value = workOrder.LastModifiedBy;
                    cmd.Parameters.Add("@LastModified", System.Data.SqlDbType.DateTime).Value = workOrder.LastModified;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = $"DELETE FROM [WORK_ORDER] WHERE Id=@Id ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task AddMaterial(int id, int woId, decimal amount)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = $"INSERT INTO [WO_MATERIAL] (WorkOrderId, InventoryItemId, UnitsUsed) " +
                             $"VALUES (@WorkOrderId, @InventoryItemId, @UnitsUsed)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@WorkOrderId", System.Data.SqlDbType.Int).Value = woId;
                    cmd.Parameters.Add("@InventoryItemId", System.Data.SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@UnitsUsed", System.Data.SqlDbType.Decimal).Value = amount;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task AddLabor(int woId, string resource, decimal cost, decimal hours)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = $"INSERT INTO [WO_LABOR] (WorkOrderId, Resource, Cost, Hours) " +
                             $"VALUES (@WorkOrderId, @Resource, @Cost, @Hours)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@WorkOrderId", System.Data.SqlDbType.Int).Value = woId;
                    cmd.Parameters.Add("@Resource", System.Data.SqlDbType.NVarChar).Value = resource;
                    cmd.Parameters.Add("@Cost", System.Data.SqlDbType.Decimal).Value = cost;
                    cmd.Parameters.Add("@Hours", System.Data.SqlDbType.Decimal).Value = hours;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
