using AutoMapper;
using Blog.Application.Models.SiteSetting;
using Blog.Domain.Entites;
using Blog.Shared.Models.Auth;

namespace Blog.Application.Mappings;

public class UserProfile : Profile
{
	public UserProfile()
	{
		CreateMap<RegisterModel, User>()
			.ForMember(dest => dest.UserName,
				opt => opt.MapFrom(src => src.PhoneNumber));

		CreateMap<UserSeed, User>();
	}
}