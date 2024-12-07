using DevTalk.Application.Posts.Commands.CreatePosts;
using DevTalk.Application.ApplicationUser.Commands.RegisterUser;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevTalk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private ApiResponse apiResponse;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
            apiResponse = new ApiResponse();
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AuthResponse>> RegisterUser([FromBody]RegisterUserCommand command)
        {
            var authResponse = await _mediator.Send(command);
            if (!authResponse.IsAuthenticated)
                return BadRequest(authResponse);
            return Ok(authResponse);
        }
    }
}
