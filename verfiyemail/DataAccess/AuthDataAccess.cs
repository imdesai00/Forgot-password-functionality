using verfiyemail.Models;
using verfiyemail.Services;
using Npgsql;
using System.Data;

namespace verfiyemail.Data
{
    public class AuthDataAccess
    {
        private readonly string _connectionstring;

        public AuthDataAccess(IConfiguration configuration)
        {
            _connectionstring = configuration.GetConnectionString("postgre");
        }
        public async Task<User> FindEmailAsync(string Email)
        {
            var users = new User();
            DataSet ds = new DataSet();
            using (var connection = new NpgsqlConnection(_connectionstring))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM \"Users\" where \"Email\" = @email";
                    command.Parameters.AddWithValue("@email", Email);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return new User
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                UserName = Convert.ToString(reader["UserName"]),
                                PhoneNo = Convert.ToInt64(reader["PhoneNo"]),
                                Email = Convert.ToString(reader["Email"]),
                                Password = Convert.ToString(reader["Password"])

                            };
                        }
                    }

                }
            }
            return null;
        }
        public async Task<bool> updatePassword(string Email, string Password)
        {
            using (var connection = new NpgsqlConnection(_connectionstring))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE \"Users\" set \"Password\" = @password where \"Email\" = @email";
                    command.Parameters.AddWithValue("@email", Email);
                    command.Parameters.AddWithValue("@password", Password);
                    command.ExecuteNonQuery();
                    return true;

                }
                return false;
            }  
        }
    }
}
