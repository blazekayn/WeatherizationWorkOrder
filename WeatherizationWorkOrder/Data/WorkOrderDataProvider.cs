using Azure.Core;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using WeatherizationWorkOrder.Data.Extensions;
using WeatherizationWorkOrder.Data.Interfaces;
using WeatherizationWorkOrder.Models;

namespace WeatherizationWorkOrder.Data
{
    public class WorkOrderDataProvider : IWorkOrderDataProvider
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public WorkOrderDataProvider(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration?.GetConnectionString("Weatherization");
        }

        public async Task<int> Create(WorkOrder item)
        {
            using SqlConnection conn = new(_connectionString);
            {
                conn.Open();
                string sql = $"INSERT INTO [WORK_ORDER] (Consumer, PreparedBy, Description, PreparedDate, LastModifiedBy, LastModified, IsComplete) " +
                             $"OUTPUT Inserted.ID " +
                             $"VALUES (@Consumer, @PreparedBy, @Description, @PreparedDate, @LastModifiedBy, @LastModified, @IsComplete)";
                using SqlCommand cmd = new(sql, conn);
                {
                    cmd.Parameters.Add("@Consumer", SqlDbType.NVarChar).Value = item.Consumer;
                    cmd.Parameters.Add("@PreparedBy", SqlDbType.NVarChar).Value = item.PreparedBy;
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = item.Description;
                    cmd.Parameters.Add("@PreparedDate", SqlDbType.DateTime).Value = item.PreparedDate;
                    cmd.Parameters.Add("@LastModifiedBy", SqlDbType.NVarChar).Value = item.LastModifiedBy;
                    cmd.Parameters.Add("@LastModified", SqlDbType.DateTime).Value = item.LastModified;
                    cmd.Parameters.Add("@IsComplete", SqlDbType.Bit).Value = item.IsComplete;


                    return (int)await cmd.ExecuteScalarAsync();
                }
            }
        }

        public async Task<WorkOrder> Read(int id)
        {
            using SqlConnection conn = new(_connectionString);
            {
                conn.Open();
                string sql = $"SELECT * FROM WORK_ORDER WHERE Id=@Id";
                using SqlCommand cmd = new(sql, conn);
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    {
                        if (dr != null && dr.HasRows)
                        {
                            await dr.ReadAsync();
                            WorkOrder workOrder = new ()
                            {
                                Id = dr.GetInt32("Id"),
                                Consumer = dr.GetString("Consumer"),
                                PreparedBy = dr.GetString("PreparedBy"),
                                Description = dr.GetString("Description"),
                                PreparedDate = dr.GetDateTime("PreparedDate"),
                                WorkDate = dr.GetNullableDateTime("WorkDate"),
                                LastModifiedBy = dr.GetString("LastModifiedBy"),
                                LastModified = dr.GetDateTime("LastModified"),
                                IsComplete = dr.GetBoolean("IsComplete")
                            };
                            return workOrder;
                        }
                    }
                }
            }
            return default;
        }

        public async Task<List<WorkOrder>> Read(bool onlyIncomplete)
        {
            List<WorkOrder> items = [];

            using SqlConnection conn = new(_connectionString);
            {
                conn.Open();
                string sql = "";
                if (onlyIncomplete)
                {
                    sql = $"SELECT * FROM WORK_ORDER WHERE IsComplete=0 ORDER BY ID DESC";
                }
                else
                {
                    sql = $"SELECT * FROM WORK_ORDER ORDER BY ID DESC";
                }
                using SqlCommand cmd = new(sql, conn);
                {
                    using SqlDataReader dr = cmd.ExecuteReader();
                    {
                        while (await dr.ReadAsync())
                        {
                            WorkOrder workOrder = new ()
                            {
                                Id = dr.GetInt32("Id"),
                                Consumer = dr.GetString("Consumer"),
                                PreparedBy = dr.GetString("PreparedBy"),
                                Description = dr.GetString("Description"),
                                PreparedDate = dr.GetDateTime("PreparedDate"),
                                WorkDate = dr.GetNullableDateTime("WorkDate"),
                                LastModifiedBy = dr.GetString("LastModifiedBy"),
                                LastModified = dr.GetDateTime("LastModified"),
                                IsComplete = dr.GetBoolean("IsComplete")
                            };
                            items.Add(workOrder);
                        }
                    }
                }
            }
            return items;
        }

        public async Task<List<WorkOrder>> Read(DateTime from, DateTime to)
        {
            List<WorkOrder> items = [];

            using SqlConnection conn = new(_connectionString);
            {
                conn.Open();
                string sql = $"SELECT * FROM WORK_ORDER WHERE WorkDate >= @from AND WorkDate <= @to ORDER BY WorkDate DESC";
                using SqlCommand cmd = new(sql, conn);
                {
                    cmd.Parameters.Add("@from", SqlDbType.DateTime).Value = from;
                    cmd.Parameters.Add("@to", SqlDbType.DateTime).Value = to;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    {
                        while (await dr.ReadAsync())
                        {
                            WorkOrder workOrder = new ()
                            {
                                Id = dr.GetInt32("Id"),
                                Consumer = dr.GetString("Consumer"),
                                PreparedBy = dr.GetString("PreparedBy"),
                                Description = dr.GetString("Description"),
                                PreparedDate = dr.GetDateTime("PreparedDate"),
                                WorkDate = dr.GetNullableDateTime("WorkDate"),
                                LastModifiedBy = dr.GetString("LastModifiedBy"),
                                LastModified = dr.GetDateTime("LastModified"),
                                IsComplete = dr.GetBoolean("IsComplete")
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
            List<Material> items = [];

            using SqlConnection conn = new (_connectionString);
            {
                conn.Open();
                string sql = $"SELECT WM.Id, I.Description, I.Cost, WM.UnitsUsed, I.Cost * WM.UnitsUsed As Total, I.Units FROM WO_MATERIAL WM JOIN INVENTORY_ITEM I ON WM.InventoryItemId = I.Id WHERE WM.WorkOrderId=@WorkOrderId";
                using SqlCommand cmd = new (sql, conn);
                {
                    cmd.Parameters.Add("@WorkOrderId", SqlDbType.Int).Value = id;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    {
                        while (await dr.ReadAsync())
                        {
                            Material item = new ()
                            {
                                Id = dr.GetInt32("Id"),
                                Description = dr.GetString("Description"),
                                CostPer = dr.GetDecimal("Cost"),
                                AmountUsed = dr.GetDecimal("UnitsUsed"),
                                Total = dr.GetDecimal("Total"),
                                Units = dr.GetString("Units"),
                            };
                            items.Add(item);
                        }
                    }
                }
            }
            return items;
        }

        public async Task<List<Labor>> ReadLabors(int id)
        {
            List<Labor> items = [];

            using SqlConnection conn = new(_connectionString);
            {
                conn.Open();
                string sql = $"SELECT *, Cost * Hours As Total FROM WO_LABOR WHERE WorkOrderId=@Id";
                using SqlCommand cmd = new(sql, conn);
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    {
                        while (await dr.ReadAsync())
                        {
                            Labor item = new ()
                            {
                                Id = dr.GetInt32("Id"),
                                WorkOrderId = dr.GetInt32("WorkOrderId"),
                                Resource = dr.GetString("Resource"),
                                Cost = dr.GetDecimal("Cost"),
                                Hours = dr.GetDecimal("Hours"),
                                Total = dr.GetDecimal("Total"),
                            };
                            items.Add(item);
                        }
                    }
                }
            }
            return items;
        }

        public async Task<Material> ReadMaterial(int id)
        {
            using SqlConnection conn = new(_connectionString);
            {
                conn.Open();
                string sql = $"SELECT WM.Id, I.Description, I.Cost, WM.UnitsUsed, I.Cost * WM.UnitsUsed As Total, I.Units, I.Id AS InventoryItemId, WM.WorkOrderId FROM WO_MATERIAL WM JOIN INVENTORY_ITEM I ON WM.InventoryItemId = I.Id WHERE WM.Id=@Id";
                using SqlCommand cmd = new (sql, conn);
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    {
                        while (await dr.ReadAsync())
                        {
                            Material item = new ()
                            {
                                Id = dr.GetInt32("Id"),
                                Description = dr.GetString("Description"),
                                CostPer = dr.GetDecimal("Cost"),
                                AmountUsed = dr.GetDecimal("UnitsUsed"),
                                Total = dr.GetDecimal("Total"),
                                Units = dr.GetString("Units"),
                                InventoryItemId = dr.GetInt32("InventoryItemId"),
                                WorkOrderId = dr.GetInt32("WorkOrderId")
                            };
                            return item;
                        }
                    }
                }
            }
            return null;
        }

        public async Task DeleteMaterial(int materialId)
        {
            using SqlConnection conn = new(_connectionString);
            {
                conn.Open();
                string sql = $"DELETE FROM [WO_MATERIAL] WHERE Id=@Id ";
                using SqlCommand cmd = new(sql, conn);
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = materialId;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        public async Task DeleteLabor(int materialId)
        {
            using SqlConnection conn = new(_connectionString);
            {
                conn.Open();
                string sql = $"DELETE FROM [WO_LABOR] WHERE Id=@Id ";
                using SqlCommand cmd = new(sql, conn);
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = materialId;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Labor> ReadLabor(int id)
        {
            using SqlConnection conn = new(_connectionString);
            {
                conn.Open();
                string sql = $"SELECT * FROM WO_LABOR WHERE Id=@Id";
                using SqlCommand cmd = new(sql, conn);
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    {
                        while (await dr.ReadAsync())
                        {
                            return new ()
                            {
                                Id = dr.GetInt32("Id"),
                                WorkOrderId = dr.GetInt32("WorkOrderId"),
                                Resource = dr.GetString("Resource"),
                                Cost = dr.GetDecimal("Cost"),
                                Hours = dr.GetDecimal("Hours"),
                            };
                        }
                    }
                }
            }
            return null;
        }

        public async Task Update(WorkOrder workOrder)
        {
            using SqlConnection conn = new(_connectionString);
            {
                conn.Open();
                string sql = $"UPDATE [WORK_ORDER] SET " +
                             $"Consumer=@Consumer, " +
                             $"PreparedBy=@PreparedBy, " +
                             $"WorkDate=@WorkDate, " +
                             $"Description=@Description, " +
                             $"LastModifiedBy=@LastModifiedBy, " +
                             $"LastModified=@LastModified, " +
                             $"IsComplete=@IsComplete " +
                             $"WHERE Id=@Id; ";
                using SqlCommand cmd = new(sql, conn);
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = workOrder.Id;
                    cmd.Parameters.Add("@Consumer", SqlDbType.NVarChar).Value = workOrder.Consumer;
                    cmd.Parameters.Add("@PreparedBy", SqlDbType.NVarChar).Value = workOrder.PreparedBy;
                    cmd.Parameters.Add("@WorkDate", SqlDbType.DateTime).Value = workOrder.WorkDate is null ? DBNull.Value : workOrder.WorkDate;
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = workOrder.Description;
                    cmd.Parameters.Add("@LastModifiedBy", SqlDbType.NVarChar).Value = workOrder.LastModifiedBy;
                    cmd.Parameters.Add("@LastModified", SqlDbType.DateTime).Value = workOrder.LastModified;
                    cmd.Parameters.Add("@IsComplete", SqlDbType.Bit).Value = workOrder.IsComplete;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task Delete(int id)
        {
            using SqlConnection conn = new(_connectionString);
            {
                conn.Open();
                string sql = $"DELETE FROM [WORK_ORDER] WHERE Id=@Id ";
                using SqlCommand cmd = new (sql, conn);
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task AddMaterial(int id, int woId, decimal amount)
        {
            using SqlConnection conn = new(_connectionString);
            {
                conn.Open();
                string sql = $"INSERT INTO [WO_MATERIAL] (WorkOrderId, InventoryItemId, UnitsUsed) " +
                             $"VALUES (@WorkOrderId, @InventoryItemId, @UnitsUsed)";
                using SqlCommand cmd = new(sql, conn);
                cmd.Parameters.Add("@WorkOrderId", SqlDbType.Int).Value = woId;
                cmd.Parameters.Add("@InventoryItemId", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@UnitsUsed", SqlDbType.Decimal).Value = amount;

                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task AddLabor(int woId, string resource, decimal cost, decimal hours)
        {
            using SqlConnection conn = new(_connectionString);
            {
                conn.Open();
                string sql = $"INSERT INTO [WO_LABOR] (WorkOrderId, Resource, Cost, Hours) " +
                             $"VALUES (@WorkOrderId, @Resource, @Cost, @Hours)";
                using SqlCommand cmd = new(sql, conn);
                cmd.Parameters.Add("@WorkOrderId", SqlDbType.Int).Value = woId;
                cmd.Parameters.Add("@Resource", SqlDbType.NVarChar).Value = resource;
                cmd.Parameters.Add("@Cost", SqlDbType.Decimal).Value = cost;
                cmd.Parameters.Add("@Hours", SqlDbType.Decimal).Value = hours;

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
