using AutoMapper;
using AutoMapper.QueryableExtensions;
using Datingnew.DTOs;
using Datingnew.Interfaces;
using Datingnew.Helpers;
using Datingnew.Models;
using Microsoft.EntityFrameworkCore;

namespace Datingnew.Data
{
	public class UserRepository : IUserRepository
	{
		private readonly DatingAppContext _context;
		private readonly IMapper _mapper;
		public UserRepository(DatingAppContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}
		public async Task<IEnumerable<User>> GetAllAsync()
		{
			return await _context.Users
				.Include(p => p.Photos)
				.ToListAsync();
		}

		public async Task<MemberDTO> GetMemberAsync(string username)
		{
			return await _context.Users
				.Where(x => x.UserName == username)
				.ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
				.SingleOrDefaultAsync();
		}

		public async Task<PagedList<MemberDTO>> GetMembersAsync(UserParams userParams)
		{
			var query = _context.Users
				.ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
				.AsNoTracking();

			return await PagedList<MemberDTO>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
		}

		public async Task<User> GetUserByIdAsync(int id)
		{
			return await _context.Users.FindAsync(id);
		}

		public async Task<User> GetUserByNameAsync(string name)
		{
			return await _context.Users
				.Include(p => p.Photos)
				.SingleOrDefaultAsync(u => u.UserName == name);
		}

		public async Task<bool> SaveAllAsync()
		{
			return await _context.SaveChangesAsync() > 0;
		}

		public void Update(User user)
		{
			_context.Entry(user).State = EntityState.Modified;
		}
	}
}
