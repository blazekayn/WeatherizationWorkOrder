﻿using Microsoft.AspNetCore.Http.Features;
using Microsoft.Data.SqlClient;
using System.Data;
using WeatherizationWorkOrder.Data.Interfaces;
using WeatherizationWorkOrder.Models;

namespace WeatherizationWorkOrder.Data
{
    public class UserDataProvider : IUserDataProvider
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public UserDataProvider(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration?.GetConnectionString("Weatherization");
        }

        public async Task Create(string name)
        {
            using SqlConnection conn = new(_connectionString);
            {
                conn.Open();
                string sql = $"INSERT INTO [WO_USER] (Name) " +
                             $"VALUES (@Name)";
                using SqlCommand cmd = new (sql, conn);
                {
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = name;


                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<User>> Read()
        {
            List<User> users = [];

            using SqlConnection conn = new(_connectionString);
            {
                conn.Open();
                string sql = $"SELECT * FROM WO_USER WHERE Deleted=0";
                sql += "ORDER BY Name";
                using SqlCommand cmd = new(sql, conn);
                {
                    using SqlDataReader dr = cmd.ExecuteReader();
                    {
                        while (await dr.ReadAsync())
                        {
                            User item = new ()
                            {
                                Id = dr.GetInt32("Id"),
                                Name = dr.GetString("Name")
                            };
                            users.Add(item);
                        }
                    }
                }
            }
            return users;
        }

        public async Task Delete(int id)
        {
            using SqlConnection conn = new(_connectionString);
            {
                conn.Open();
                string sql = $"DELETE FROM [WO_USER] WHERE Id=@Id ";
                using SqlCommand cmd = new(sql, conn);
                {
                    cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task Update(int id, string name)
        {
            using SqlConnection conn = new(_connectionString);
            {
                conn.Open();
                string sql = $"UPDATE WO_USER SET name=@name WHERE Id=@Id";
                using SqlCommand cmd = new(sql, conn);
                {
                    cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@name", System.Data.SqlDbType.NVarChar).Value = name;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
