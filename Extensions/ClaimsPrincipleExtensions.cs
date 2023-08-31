using System.Security.Claims;

namespace Datingnew.Extensions
{
	public static class ClaimsPrincipleExtensions
	{
		public static string GetUserName(this ClaimsPrincipal User)
		{
			return User.FindFirst(ClaimTypes.Name)?.Value;
		}

		public static string GetUserId(this ClaimsPrincipal User)
		{
			return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		}
	}
}
