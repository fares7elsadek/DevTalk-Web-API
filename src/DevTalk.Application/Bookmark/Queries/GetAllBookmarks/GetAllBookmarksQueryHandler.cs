using AutoMapper;
using DevTalk.Application.ApplicationUser;
using DevTalk.Application.Bookmark.Dtos;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Bookmark.Queries.GetAllBookmarks;

public class GetAllBookmarksQueryHandler(
    IUnitOfWork unitOfWork,IMapper mapper) : IRequestHandler<GetAllBookmarksQuery, IEnumerable<BookmarkDto>>
{
    public async Task<IEnumerable<BookmarkDto>> Handle(GetAllBookmarksQuery request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        var user = await unitOfWork.User.GetOrDefalutAsync(x => x.Id == userId,
            IncludeProperties:"Bookmarks");
        if (user is null)
            throw new CustomeException("Something wrong has happened");

        if (!user.Bookmarks.Any())
            throw new CustomeException("There's no bookmarks yet!");

        var bookmarksDto = mapper.Map<IEnumerable<BookmarkDto>>(user.Bookmarks);
        return bookmarksDto;
    }
}
