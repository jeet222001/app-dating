using AutoMapper;
using Datingnew.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Datingnew.Models;
using Datingnew.DTOs;
using System.Security.Cryptography;
using System.Text;

namespace Datingnew.Controllers;

public class AccountController : BaseApiController
{
	private readonly DatingAppContext _context;
	private readonly ITokenService _tokenService;
	private readonly IMapper _mapper;

	public AccountController(DatingAppContext context, ITokenService tokenService, IMapper mapper)
	{
		_context = context;
		_tokenService = tokenService;
		_mapper = mapper;
	}

	[HttpPost("register")]

	public async Task<ActionResult<UserDTO>> Register(RegisterDTO register)
	{
		if (await UserExists(register.Username))
		{
			return BadRequest("Username is Taken");
		}

		var user = _mapper.Map<User>(register);

		using var hmac = new HMACSHA512();

		user.UserName = register.Username.ToLower();
		user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.Password));
		user.PasswordSalt = hmac.Key;

		_context.Users.Add(user);
		await _context.SaveChangesAsync();
		return Ok(new UserDTO
		{
			Username = user.UserName,
			Token = _tokenService.CreateToken(user),
			PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
			KnownAs = user.KnownAs,
			Gender = user.Gender,
		});
	}
	[HttpPost("login")]
	public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
	{
		var user = await _context.Users
			.Include(x => x.Photos)
			.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

		if (user == null) return Unauthorized("Invalid Username");

		using var hmac = new HMACSHA512(user.PasswordSalt);

		var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

		for (int i = 0; i < computedHash.Length; i++)
		{
			if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
		}
		return Ok(new UserDTO
		{
			Username = user.UserName,
			Token = _tokenService.CreateToken(user),
			PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
			KnownAs = user.KnownAs,
			Gender = user.Gender,
		});

	}

	private async Task<bool> UserExists(string username)
	{
		return await _context.Users.AnyAsync(x => x.UserName == username);
	}
}