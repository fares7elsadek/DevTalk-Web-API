using AutoMapper;
using DevTalk.Domain.Entites;

namespace DevTalk.Application.Bookmark.Dtos;

public class BookmarkProfile:Profile
{
    public BookmarkProfile()
    {
        CreateMap<Bookmarks,BookmarkDto>()
            .ForMember(x => x.Post,options =>
            {
                options.MapFrom(x => x.Post);
            }).ReverseMap();
    }
}
