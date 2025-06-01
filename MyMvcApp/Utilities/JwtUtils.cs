using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyMvcApp.Utilities
{
    public static class JwtUtils
    {
        public static IEnumerable<Claim> DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Claims;
        }

        public static bool IsTokenValid(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(token))
                return false;

            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.ValidTo > DateTime.UtcNow;
        }

        public static string? GetClaimValue(string token, string claimType)
        {
            var claims = DecodeToken(token);
            return claims.FirstOrDefault(c => c.Type == claimType)?.Value;
        }

        public static string? GetAccessTokenFromSession(HttpContext context)
        {
            return context.Session.GetString("AccessToken");
        }
    }
}
