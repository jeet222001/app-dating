using Datingnew.DTOs;
using Datingnew.Helpers;
using Datingnew.Models;

namespace Datingnew.Interfaces
{
	public interface IUserRepository
	{
		void Update(User user);
		Task<bool> SaveAllAsync();
		Task<IEnumerable<User>> GetAllAsync();
		Task<User> GetUserByIdAsync(int id);
		Task<User> GetUserByNameAsync(string name);
		Task<PagedList<MemberDTO>> GetMembersAsync(UserParams userParams);
		Task<MemberDTO> GetMemberAsync(string username);
	}
}
