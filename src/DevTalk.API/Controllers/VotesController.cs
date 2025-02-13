using DevTalk.Application.Comments.Commands.CreateComment;
using DevTalk.Application.PostVote.Commands.CreatePostVoteDownVote;
using DevTalk.Application.PostVote.Commands.CreatePostVoteUpVote;
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
    public class VotesController : ControllerBase
    {

        private readonly IMediator _mediator;
        public VotesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("upvote")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpVote([FromRoute] string PostId)
        {
            await _mediator.Send(new CreatePostUpVoteCommand(PostId));
            return Created();
        }

        [HttpPost("downvote")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DownVote([FromRoute] string PostId)
        {
            await _mediator.Send(new CreatePostDownVoteCommand(PostId));
            return Created();
        }
    }
}
