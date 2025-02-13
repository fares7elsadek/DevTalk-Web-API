using DevTalk.Application.Comments.Commands.CreateComment;
using DevTalk.Application.Comments.Commands.DeleteComment;
using DevTalk.Application.Comments.Commands.UpdateComment;
using DevTalk.Application.Comments.Queries.GetAllCommentsByPost;
using DevTalk.Application.Comments.Queries.GetCommentById;
using DevTalk.Application.Posts.Commands.CreatePosts;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DevTalk.API.Controllers
{
    [Route("api/{PostId}/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {

        private readonly IMediator _mediator;
        private ApiResponse apiResponse;
        public CommentController(IMediator mediator)
        {
            _mediator = mediator;
            apiResponse = new ApiResponse();
        }


        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateComment([FromBody]CreateCommentCommand command,[FromRoute]string PostId)
        {
            var newCommand = new CreateCommentCommand(PostId);
            newCommand.CommentText = command.CommentText;
            await _mediator.Send(newCommand);
            return Created();
        }


        [HttpDelete("{CommentId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> DeleteComment([FromRoute] string CommentId, [FromRoute] string PostId)
        {
            var command = new DeleteCommentCommand(CommentId,PostId);
            await _mediator.Send(command);
            apiResponse.IsSuccess = true;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.Result = "Comment deleted successfully";
            return Ok(apiResponse);
        }

        [HttpPatch("{CommentId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> UpdateComment([FromRoute] string CommentId, [FromRoute] string PostId,
            [FromBody] UpdateCommentCommand command)
        {
            var newCommand = new UpdateCommentCommand(CommentId, PostId);
            newCommand.CommentText = command.CommentText;
            await _mediator.Send(newCommand);
            apiResponse.IsSuccess = true;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.Result = "Comment updated successfully";
            return Ok(apiResponse);
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetAllPostComments([FromRoute] string PostId,
            [FromQuery] int page = 1, [FromQuery] int size = 5)
        {
            var comments = await _mediator.Send(new GetAllCommentsByPostQuery(PostId,page,size));
            apiResponse.IsSuccess = true;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.Result = comments;
            return Ok(apiResponse);
        }


        [HttpGet("{CommentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetCommentById([FromRoute] string CommentId, [FromRoute] string PostId)
        {
            var comment = await _mediator.Send(new GetCommentByIdQuery(CommentId,PostId));
            apiResponse.IsSuccess = true;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.Result = comment;
            return Ok(apiResponse);
        }
    }
}
