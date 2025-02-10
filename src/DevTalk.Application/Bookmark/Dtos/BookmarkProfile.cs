using AutoMapper;
using DevTalk.Domain.Entites;

namespace DevTalk.Application.Bookmark.Dtos;

public class BookmarkProfile:Profile
{
    public BookmarkProfile()
    {
        CreateMap<Bookmarks,BookmarkDto>().ReverseMap();
    }
}
