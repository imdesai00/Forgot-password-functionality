using verfiyemail.Controllers;
using verfiyemail.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Collections;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace verfiyemail.Services
{

    public class TokenService
    {
        private readonly string _connectionstring;
        private readonly EmailService _emailService;
        private string _token;

        public TokenService(IConfiguration configuration ,EmailService emailService)
        {
            _connectionstring = configuration.GetConnectionString("postgre");
            _emailService = emailService;
        }


        public async Task<string> GenerateTokenAsync(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("abcdefghijklmnopqrstuvwxyzsuperSecretKey@345");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("Email", user.Email) }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public async Task<bool> StoreTokenAsync(User user, string token)
        {
            var tokenEntry = new Token
            {
                Email = user.Email,
                ResetToken = token,
                ExpiryDate = DateTime.UtcNow.AddMinutes(10)// Example: token expires in 1 hour
            };
            using (var connection = new NpgsqlConnection(_connectionstring))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("insert into \"ResetTokens\" (\"Email\",\"ResetToken\",\"ExpiryDate\") values (@email,@token,@expiredate);", connection))
                {
                    command.Parameters.AddWithValue("@email", tokenEntry.Email);
                    command.Parameters.AddWithValue("@token", tokenEntry.ResetToken);
                    command.Parameters.AddWithValue("@expiredate", tokenEntry.ExpiryDate);
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }
        public async Task<bool> ValidateTokenAsync(User user, string token)
        {
            var valid = new Token();
            using (var connection = new NpgsqlConnection(_connectionstring))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM \"ResetTokens\" where \"Email\" = @email and \"ResetToken\" = @token";
                    command.Parameters.AddWithValue("@email", user.Email);
                    command.Parameters.AddWithValue("@token", token);
                    using (var reader = command.ExecuteReader())
                    {
                        
                        while (reader.Read())
                        {
                            var tokenEntry = new Token
                            {
                                Email = Convert.ToString(reader["Email"]),
                                ResetToken = Convert.ToString(reader["ResetToken"]),
                                ExpiryDate = Convert.ToDateTime(reader["ExpiryDate"])

                            };
                            valid = tokenEntry;

                        }
                    }
                    if (valid == null || valid.ExpiryDate < DateTime.UtcNow)
                    {
                        return false;
                    }
                    else
                    {
                        command.CommandText = "DELETE FROM \"ResetTokens\" where \"Email\" = @email";
                        command.Parameters.AddWithValue("@email", user.Email);
                        command.ExecuteNonQuery();
                        return true;
                    }
                }
            }

        }
    }
}
