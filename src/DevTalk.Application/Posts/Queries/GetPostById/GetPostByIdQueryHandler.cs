using System.Data;
using AutoMapper;
using Dapper;
using DevTalk.Application.Posts.Dtos;
using DevTalk.Domain.Abstractions;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Exceptions;
using MediatR;

namespace DevTalk.Application.Posts.Queries.GetPostById;

internal class GetPostByIdQueryHandler(IMapper mapper
    ,ISqlConnectionFactory dapper) : IRequestHandler<GetPostByIdQuery, PostDto>
{
    public async Task<PostDto> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var sql = @"
                    -- get post by id
                    SELECT * FROM ""Posts"" WHERE ""PostId"" = @Id;
                ";
        using IDbConnection connection = dapper.CreateConnection();
        var post = await connection.QueryFirstAsync<Post>(sql, new { id = request.PostId });
        if (post == null) throw new NotFoundException(nameof(post), request.PostId);
        var PostDto = mapper.Map<PostDto>(post);
        return PostDto;
    }
}
