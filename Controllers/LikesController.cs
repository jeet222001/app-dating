using Datingnew.DTOs;
using Datingnew.Extensions;
using Datingnew.Helpers;
using Datingnew.Interfaces;
using Datingnew.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Datingnew.Controllers
{
	public class LikesController : BaseApiController
	{
		private readonly IUserRepository _userRepository;
		private readonly ILikesRepository _likesRepository;

		public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
		{
			_likesRepository = likesRepository;
			_userRepository = userRepository;
		}

		[HttpPost("{username}")]
		public async Task<ActionResult> AddLike(string username)
		{
			var sourceUserId = int.Parse(User.GetUserId());
			var likedUser = await _userRepository.GetUserByNameAsync(username);
			var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);

			if (likedUser == null) return NotFound();
			if (sourceUser.UserName == username) return BadRequest("You Cannnot Like Yourself");

			var userLike = await _likesRepository.GetUserLike(sourceUserId, likedUser.Id);
			if (userLike != null) return BadRequest("you already Like this user");
			userLike = new UserLike
			{
				SourceId = sourceUserId,
				TargetUserId = likedUser.Id,
			};
			sourceUser.LikedUsers.Add(userLike);

			if (await _userRepository.SaveAllAsync()) return Ok();
			return BadRequest("failed to like user");
		}

		[HttpGet]
		public async Task<ActionResult<PagedList<LikeDTO>>> GetUserLikes([FromQuery] LikesParams likesParams)
		{
			likesParams.UserId = int.Parse(User.GetUserId());
			var users = await _likesRepository.GetUserLikes(likesParams);

			Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));

			return Ok(users);
		}
	}
}