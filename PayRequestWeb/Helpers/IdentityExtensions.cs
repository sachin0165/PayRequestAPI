using System.Security.Claims;

namespace PayRequestWeb.Helpers
{
    public static class IdentityExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return null;

            var currentUser = user;
            return currentUser.FindFirst(c => c.Type == ApiConstants.JwtRegisteredClaimNamesUserId).Value;
        }


        public static string GetUserWalletAddress(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return null;

            var currentUser = user;
            return currentUser.FindFirst(c => c.Type == ApiConstants.JwtRegisteredClaimNamesUserWalletAddress).Value;
        }
    }
}
