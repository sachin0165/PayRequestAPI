using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PayRequestWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PayRequestWeb.DataService
{
    public class AuthService : IAuthService
    {
        private readonly IDatabaseSettings _databaseSettings;
        private readonly IConfiguration _configuration;

        public AuthService(IDatabaseSettings databaseSettings,
            IConfiguration configuration)
        {
            _databaseSettings = databaseSettings;
            _configuration = configuration;
        }

        public async Task<User> Login(LoginRequest loginRequest)
        {
            using (IDbConnection con = new SqlConnection(_databaseSettings.ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@Email", loginRequest.Email);
                parameter.Add("@Password", loginRequest.Password);
                var result = await con.QueryFirstOrDefaultAsync<User>("LoginUser", parameter, commandType: CommandType.StoredProcedure, commandTimeout: 600);
                return result;
            }
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("General")["PrivateKey"])), SecurityAlgorithms.HmacSha256Signature);

            var tokenExpiry = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["General:TokenExpiryMinutes"]));

            var jwt = new JwtSecurityToken(
                        signingCredentials: signingCredentials,
                        claims: claims,
                        notBefore: DateTime.UtcNow,
                        expires: tokenExpiry,
                        audience: "Anyone",
                        issuer: "Anyone"
                    );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
