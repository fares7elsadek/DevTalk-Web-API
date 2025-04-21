using System.Data;
using AutoMapper;
using Dapper;
using DevTalk.Application.Bookmark.Dtos;
using DevTalk.Domain.Abstractions;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Bookmark.Queries.GetBookmarkById;

public class GetBookmarkByIdQueryHandler(
    IMapper mapper,ISqlConnectionFactory dapper) : IRequestHandler<GetBookmarkByIdQuery, BookmarkDto>
{
    public async Task<BookmarkDto> Handle(GetBookmarkByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        var sql = @"
                    -- get bookmark by id
                    SELECT * FROM ""Bookmarks"" WHERE ""UserId"" = @userId
                ";
        using IDbConnection connection = dapper.CreateConnection();
        var bookmark = await connection.QueryFirstAsync<Bookmarks>(sql,new { userId });
        if (bookmark is null)
            throw new NotFoundException(nameof(bookmark),request.BookmarkId);
        var bookmarkDto = mapper.Map<BookmarkDto>(bookmark);
        return bookmarkDto;
    }
}
