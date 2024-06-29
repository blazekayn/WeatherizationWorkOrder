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

               // string sql = "INSERT INTO[WORK_ORDER](Consumer, PreparedBy, Description, PreparedDate, LastModifiedBy, LastModified) " +
                 //            "VALUES('Aaron', 'Tina', 'oijpoij', '01/01/2024', 'Aaron', '06/19/2024')";
                string sql = $"INSERT INTO [WORK_ORDER] (Consumer, PreparedBy, Description, PreparedDate, LastModifiedBy, LastModified) " +
                             $"VALUES (@Consumer, @PreparedBy, @Description, @PreparedDate, @LastModifiedBy, @LastModified)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Consumer", System.Data.SqlDbType.NVarChar).Value = item.Consumer;
                    cmd.Parameters.Add("@PreparedBy", System.Data.SqlDbType.NVarChar).Value = item.PreparedBy;
                    cmd.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar).Value = item.Description;
                    cmd.Parameters.Add("@PreparedDate", System.Data.SqlDbType.DateTime).Value = item.PreparedDate;
                    cmd.Parameters.Add("@LastModifiedBy", System.Data.SqlDbType.NVarChar).Value = item.LastModifiedBy;
                    cmd.Parameters.Add("@LastModified", System.Data.SqlDbType.DateTime).Value = item.LastModified;

                    await cmd.ExecuteNonQueryAsync();
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

        public async Task Update(WorkOrder workOrder)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = $"UPDATE [WORK_ORDER] SET " +
                             $"Consumer=@Consumer, " +
                             $"PreparedBy=@PreparedBy, " +
                             $"Description=@Description, " +
                             $"PreparedDate=@PreparedDate, " +
                             $"LastModifiedBy=@LastModifiedBy, " +
                             $"LastModified=@LastModified";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Consumer", System.Data.SqlDbType.NVarChar).Value = workOrder.Consumer;
                    cmd.Parameters.Add("@PreparedBy", System.Data.SqlDbType.NVarChar).Value = workOrder.PreparedBy;
                    cmd.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar).Value = workOrder.Description;
                    cmd.Parameters.Add("@PreparedDate", System.Data.SqlDbType.DateTime).Value = workOrder.PreparedDate;
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

        public async Task AddMaterial(string id, int woId, decimal amount)
        {

        }
    }
}
