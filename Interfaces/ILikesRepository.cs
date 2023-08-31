using Datingnew.DTOs;
using Datingnew.Helpers;
using Datingnew.Models;

namespace Datingnew.Interfaces
{
	public interface ILikesRepository
	{
		Task<UserLike> GetUserLike(int sourceUserId, int TargetUserid);
		Task<User> GetUserWithLikes(int userId);
		Task<PagedList<LikeDTO>> GetUserLikes(LikesParams likesParams);
	}
}
