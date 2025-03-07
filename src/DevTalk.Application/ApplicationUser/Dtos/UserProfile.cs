using AutoMapper;
using DevTalk.Application.ApplicationUser.Commands.RegisterUser;
using DevTalk.Domain.Entites;

namespace DevTalk.Application.ApplicationUser.Dtos;

public class UserProfile:Profile
{
    public UserProfile()
    {
        CreateMap<User, RegisterUserCommand>()
        .ForMember(dest => dest.Password, opt => opt.Ignore())
        .ReverseMap();

        CreateMap<User, UserDto>()
            .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
        .ReverseMap();
    }
}
