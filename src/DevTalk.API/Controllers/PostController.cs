using DevTalk.Application.Posts.Commands.CreatePosts;
using DevTalk.Application.Posts.Commands.DeletePost;
using DevTalk.Application.Posts.Commands.UpdatePosts;
using DevTalk.Application.Posts.Dtos;
using DevTalk.Application.Posts.Queries.GetAllPosts;
using DevTalk.Application.Posts.Queries.GetFeedPosts;
using DevTalk.Application.Posts.Queries.GetPostById;
using DevTalk.Application.Posts.Queries.GetTrendingPosts;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetAllPosts(
            [FromQuery] string cursor = "",
            [FromQuery]int pageSize = 10)
        {
            if(pageSize < 0) pageSize = 10;
            var result = await _mediator.Send(new GetAllPostsQuery(pageSize,cursor));
            apiResponse.IsSuccess=true;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.Result = result;
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

        [HttpPost("create")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreatePost([FromForm]CreatePostCommand command)
        {
            var PostDto = await _mediator.Send(command);
            if (PostDto == null) throw new CustomeException("Something wrong has happened");
            var PostId = PostDto.PostId;
            return CreatedAtAction(nameof(GetPostById),new { PostId },null);
        }

        [HttpPatch("update")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> UpdatePost([FromBody]UpdatePostsCommand command)
        {
            await _mediator.Send(command);
            apiResponse.IsSuccess = true;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.Result = "Post updated successfully";
            return Ok(apiResponse);
        }


        [HttpDelete("delete/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> DeletePost([FromRoute]string id)
        {
            await _mediator.Send(new DeletePostCommand(id));
            apiResponse.IsSuccess = true;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.Result = "Post deleted successfully";
            return Ok(apiResponse);
        }

        [HttpGet("trending")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetTrendingPosts([FromQuery] string timeCursor = ""
            , [FromQuery] double scoreCursor = 0,
            [FromQuery] string idCursor = "", [FromQuery] int size = 5
            )
        {
            var result = await _mediator.Send(new 
                GetTrendingPostsQuery(idCursor,timeCursor,scoreCursor,size));
            apiResponse.IsSuccess = true;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.Result = result;
            return Ok(apiResponse);
        }

        [HttpGet("feed")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetFeedPosts([FromQuery] string timeCursor = ""
            , [FromQuery] double scoreCursor = 0,
            [FromQuery] string idCursor = "", [FromQuery] int size = 5
            )
        {
            var userId = User.FindFirst(c => c.Type == "uid")!.Value;
            var result = await _mediator.Send(new
                GetFeedPostsQuery(userId,idCursor, timeCursor, scoreCursor, size));
            apiResponse.IsSuccess = true;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.Result = result;
            return Ok(apiResponse);
        }

        [HttpGet("category/{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetAllCategoryPosts([FromRoute] string categoryId,[FromQuery] string timeCursor = ""
            , [FromQuery] double scoreCursor = 0,
            [FromQuery] string idCursor = "", [FromQuery] int size = 5
            )
        {
            var userId = User.FindFirst(c => c.Type == "uid")!.Value;
            var result = await _mediator.Send(new
                GetFeedPostsQuery(userId, idCursor, timeCursor, scoreCursor, size));
            apiResponse.IsSuccess = true;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.Result = result;
            return Ok(apiResponse);
        }
    }
}
