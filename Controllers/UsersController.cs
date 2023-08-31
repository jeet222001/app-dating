using AutoMapper;
using Datingnew.DTOs;
using Datingnew.Extensions;
using Datingnew.Interfaces;
using Datingnew.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Datingnew.Helpers;

namespace Datingnew.Controllers
{
	[Authorize]
	public class UsersController : BaseApiController
	{
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;
		private readonly IPhotoService _photoService;
		public UsersController(
			IUserRepository userRepository,
			IMapper mapper,
			IPhotoService photoService)
		{
			_userRepository = userRepository;
			_mapper = mapper;
			_photoService = photoService;
		}

		[HttpGet]
		public async Task<ActionResult<PagedList<MemberDTO>>> GetUsers([FromQuery] UserParams userParams)
		{
			var currentUser = await _userRepository.GetUserByNameAsync(User.GetUserName());
			userParams.CurrentUserName = currentUser.UserName;

			if (string.IsNullOrEmpty(currentUser.Gender))
			{
				userParams.Gender = currentUser.Gender == "male" ? "female" : "male";
			}

			var users = await _userRepository.GetMembersAsync(userParams);
			Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));
			return Ok(users);
		}

		[HttpGet("{username}")]
		public async Task<ActionResult<MemberDTO>> GetUser(string username)
		{
			return await _userRepository.GetMemberAsync(username);
		}

		[HttpPut]
		public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdateDTO)
		{
			var user = await _userRepository.GetUserByNameAsync(User.GetUserName());
			if (user == null) return NotFound();

			_mapper.Map(memberUpdateDTO, user);

			if (await _userRepository.SaveAllAsync()) return NoContent();

			return BadRequest("failed to update the user");
		}

		[HttpPost("add-photo")]
		public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
		{
			var user = await _userRepository.GetUserByNameAsync(User.GetUserName());
			if (user == null) return NotFound();

			var result = await _photoService.AddPhotoAsync(file);
			if (result.Error != null) return BadRequest(result.Error.Message);
			var Photo = new Photo
			{
				Url = result.SecureUrl.AbsoluteUri,
				PublicId = result.PublicId,
			};
			if (user.Photos.Count == 0) Photo.IsMain = true;
			user.Photos.Add(Photo);

			if (await _userRepository.SaveAllAsync())
			{
				return CreatedAtAction(nameof(GetUser),
					new { username = user.UserName }, _mapper.Map<PhotoDto>(Photo));
			}

			return BadRequest("Problem Adding Photo");
		}

		[HttpPut("set-main-photo/{photoId}")]
		public async Task<ActionResult> setMainPhoto(int photoId)
		{
			var user = await _userRepository.GetUserByNameAsync(User.GetUserName());
			if (user == null) return NotFound();
			var photo = user.Photos.FirstOrDefault(u => u.Id == photoId);
			if (photo == null) return NotFound();
			if (photo.IsMain) return BadRequest("this is already you main photo");

			var currentMain = user.Photos.FirstOrDefault(u => u.IsMain);
			if (currentMain != null) currentMain.IsMain = false;
			photo.IsMain = true;

			if (await _userRepository.SaveAllAsync()) return NoContent();

			return BadRequest("Problem setting the main photo");
		}

		[HttpDelete("delete-photo/{photoId}")]

		public async Task<ActionResult> DeletePhoto(int photoId)
		{
			var user = await _userRepository.GetUserByNameAsync(User.GetUserName());

			var photo = user.Photos.FirstOrDefault(u => u.Id == photoId);

			if (photo == null) return NotFound();

			if (photo.IsMain) return BadRequest("You can not delete your main photo");

			if (photo.PublicId != null)
			{
				var result = await _photoService.DeletePhotoAsync(photo.PublicId);
				if (result.Error != null) return BadRequest(result.Error.Message);
			}
			user.Photos.Remove(photo);

			if (await _userRepository.SaveAllAsync()) return Ok();

			return BadRequest("Error deleting photo");
		}
	}
}