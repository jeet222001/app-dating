using Datingnew.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Datingnew.Data
{
	public class SeedData
	{
		public static async Task SeedUsers(DatingAppContext context)
		{
			if (await context.Users.AnyAsync()) return;

			var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

			var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

			var users = JsonSerializer.Deserialize<List<User>>(userData, options); 

			foreach(var user in users)
			{
				using var hmac = new HMACSHA512();
				user.UserName = user.UserName.ToLower();
				user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
				user.PasswordSalt = hmac.Key;
				context.Users.Add(user);
			}
			await context.SaveChangesAsync();
		}
	}
}
