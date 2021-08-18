using Microsoft.AspNetCore.Mvc;
using PayRequestWeb.DataService;
using PayRequestWeb.Helpers;
using PayRequestWeb.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PayRequestWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _authService.Login(loginRequest);
            if (user == null)
                return BadRequest("Invalid email or password");

            var usersClaims = new[]
            {
                    new Claim(ApiConstants.JwtRegisteredClaimNamesUserId,  user.Id.ToString()),
                    new Claim(ApiConstants.JwtRegisteredClaimNamesUserWalletAddress,  user.WalletAddress)
            };

            var jwtToken = _authService.GenerateAccessToken(usersClaims);

            return Success(new LoginResponse
            {
                AccessToken = jwtToken,
                WalletAddress = user.WalletAddress
            });
        }
    }
}