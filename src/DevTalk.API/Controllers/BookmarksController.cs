using DevTalk.Application.Bookmark.commands.CreateBookmark;
using DevTalk.Application.Bookmark.commands.DeleteBookmark;
using DevTalk.Application.Bookmark.Queries.GetAllBookmarks;
using DevTalk.Application.Bookmark.Queries.GetBookmarkById;
using DevTalk.Domain.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DevTalk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookmarksController : ControllerBase
    {
        private readonly IMediator _mediator;
        private ApiResponse apiResponse;
        public BookmarksController(IMediator mediator)
        {
            _mediator = mediator;
            apiResponse = new ApiResponse();
        }

        [HttpGet("all")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetAllBookmarks(
            [FromQuery] int page = 1, [FromQuery] int size = 5)
        {
            var userId = User.FindFirst(c => c.Type == "uid")!.Value;
            if (page < 0) page = 1;
            var bookmarks = await _mediator.Send(new GetAllBookmarksQuery(userId));
            int total = bookmarks.Count();
            if (size > total) size = 5;
            int pages = (int)Math.Ceiling((decimal)total / size);
            if (page > pages)
            {
                page = pages;
            }
            var result = bookmarks.Skip((page - 1) * size).Take(size).ToList();
            apiResponse.IsSuccess = true;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.Result = result;
            return Ok(apiResponse);
        }

        

        [HttpGet("{bookmarkId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetBookmark(
            [FromRoute] string bookmarkId)
        {
            var userId = User.FindFirst(c => c.Type == "uid")!.Value;
            var bookmark = await _mediator.Send(new GetBookmarkByIdQuery(userId,bookmarkId));
            apiResponse.IsSuccess = true;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.Result = bookmark;
            return Ok(apiResponse);
        }

        [HttpPost("{postId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateBookmark([FromRoute] string postId)
        {
            var command = new CreateBookmarkCommand(postId);
            await _mediator.Send(command);
            return Created();
        }

        [HttpDelete("{postId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteBookmark(string postId)
        {
            await _mediator.Send(new DeleteBookmarkCommand(postId));
            return Ok();
        }
    }
}
