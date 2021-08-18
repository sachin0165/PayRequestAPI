using PayRequestWeb.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PayRequestWeb.DataService
{
    public interface IAuthService
    {
        Task<User> Login(LoginRequest loginRequest);

        string GenerateAccessToken(IEnumerable<Claim> claims);
    }
}
