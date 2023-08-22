using AutoMapper;
using Datingnew.DTOs;
using Datingnew.Extensions;
using Datingnew.Models;

namespace Datingnew.Helpers
{
	public class AutoMapperProfiles : Profile
	{
		public AutoMapperProfiles()
		{
			CreateMap<User, MemberDTO>()
				  .ForMember(dest => dest.PhotoUrl,
					  opt => opt.MapFrom(src => src.Photos.FirstOrDefault(u => u.IsMain == true).Url))
				  .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
			CreateMap<Photo, PhotoDto>();
			CreateMap<MemberUpdateDTO, User>();
			CreateMap<RegisterDTO, User>();
		}
	}
}
