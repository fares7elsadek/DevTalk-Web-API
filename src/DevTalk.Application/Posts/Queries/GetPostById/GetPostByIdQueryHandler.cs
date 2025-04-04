﻿using AutoMapper;
using DevTalk.Application.Posts.Dtos;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Posts.Queries.GetPostById;

internal class GetPostByIdQueryHandler(IMapper mapper,
    IUnitOfWork unitOfWork) : IRequestHandler<GetPostByIdQuery, PostDto>
{
    public async Task<PostDto> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var Post = await unitOfWork.Post.GetPostById(request.PostId);
        if (Post == null) throw new NotFoundException(nameof(Post), request.PostId);
        var PostDto = mapper.Map<PostDto>(Post);
        return PostDto;
    }
}
