using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PayRequestWeb.Helpers;
using PayRequestWeb.Models;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PayRequestWeb.EventHandler
{
    public class JwtBearerEventsWriter
    {
        public static Task SetTokenExpiryResponse(AuthenticationFailedContext context)
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = MediaTypeNames.Application.Json;

                context.Response.Headers.Add("Token-Expired", "true");

                var errorResponse = new SuccessResponse
                {
                    Message = "Token expired"
                };

                using (TextWriter writer = new HttpResponseStreamWriter(context.Response.Body, Encoding.UTF8))
                {
                    writer.Write(JsonConvert.SerializeObject(errorResponse));
                }
            }
            return Task.CompletedTask;
        }

        public static Task SetUserIdentity(TokenValidatedContext context)
        {
            var claims = context.Principal.Claims;

            var userIdentity = new UserIdentity
            {
                UserId = Convert.ToInt32(claims.Where(c => c.Type == ApiConstants.JwtRegisteredClaimNamesUserId).Select(c => c.Value).SingleOrDefault()),
                WalletAddress = claims.Where(c => c.Type == ApiConstants.JwtRegisteredClaimNamesUserWalletAddress).Select(c => c.Value).SingleOrDefault(),
            };

            context.HttpContext.User = new GenericPrincipal(userIdentity, Array.Empty<string>());
            return Task.CompletedTask;
        }
    }
}
