using Datingnew.DTOs;
using Datingnew.Extensions;
using Datingnew.Helpers;
using Datingnew.Interfaces;
using Datingnew.Models;
using Microsoft.EntityFrameworkCore;

namespace Datingnew.Data
{
	public class LikedRepository : ILikesRepository
	{
		private readonly DatingAppContext _Context;
		public LikedRepository(DatingAppContext datingAppContext)
		{
			_Context = datingAppContext;

		}

		public async Task<UserLike> GetUserLike(int sourceUserId, int TargetUserid)
		{
			return await _Context.Likes.FindAsync(sourceUserId, TargetUserid);
		}

		public async Task<PagedList<LikeDTO>> GetUserLikes(LikesParams likesParams)
		{
			var users = _Context.Users.OrderBy(u => u.UserName).AsQueryable();
			var likes = _Context.Likes.AsQueryable();
			if (likesParams.Predicate == "liked")
			{
				likes = likes.Where(like => like.SourceId == likesParams.UserId);
				users = likes.Select(like => like.TargetUser);
			}
			if (likesParams.Predicate == "likedBy")
			{
				likes = likes.Where(like => like.SourceId == likesParams.UserId);
				users = likes.Select(like => like.TargetUser);
			}

			var LikedUsers = users.Select(user =>
					new LikeDTO
					{
						UserName = user.UserName,
						KnownAs = user.KnownAs,
						Age = user.DateOfBirth.CalculateAge(),
						PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain).Url,
						City = user.City,
						Id = user.Id,
					});
			return await PagedList<LikeDTO>.CreateAsync(LikedUsers, likesParams.PageNumber, likesParams.PageSize);
		}

		public async Task<User> GetUserWithLikes(int userId)
		{
			return await _Context.Users
				.Include(x => x.LikedUsers)
				.FirstOrDefaultAsync(x => x.Id == userId);
		}
	}
}
