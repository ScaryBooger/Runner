using System.Security.Claims;

namespace WebApplication1
{
    public static class CliamsPrincipalExtenstions
    {
        public static string GetUserId(this ClaimsPrincipal user) {

            return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

    }
}
