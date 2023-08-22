using Datingnew.Models;

namespace Datingnew.Interfaces
{
	public interface ITokenService
	{
		string CreateToken(User user);
	}
}
