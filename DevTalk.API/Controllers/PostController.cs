using DevTalk.Application.Posts.Dtos;
using DevTalk.Application.Posts.Queries.GetAllPosts;
using DevTalk.Application.Posts.Queries.GetPostById;
using DevTalk.Domain.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DevTalk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;
        private ApiResponse apiResponse;
        public PostController(IMediator mediator)
        {
            _mediator = mediator;
            apiResponse = new ApiResponse();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetAllPosts(
            [FromQuery]int page=1, [FromQuery]int size = 5)
        {
            if(page < 0) page = 1;
            var Posts = await _mediator.Send(new GetAllPostsQuery());
            int total = Posts.Count();
            if (size > total) size = 5;
            int pages = (int)Math.Ceiling((decimal)total / size);
            if (page > pages)
            {
                page = pages;
            }
            var ResultPosts = Posts.Skip((page - 1) * size).Take(size).ToList();
            apiResponse.IsSuccess=true;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.Result = ResultPosts;
            return Ok(apiResponse);
        }
        [HttpGet("{PostId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetPostById(string PostId)
        {
            var Post = await _mediator.Send(new GetPostByIdQuery(PostId));
            apiResponse.IsSuccess = true;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.Result = Post;
            return Ok(apiResponse);
        }
    }
}
