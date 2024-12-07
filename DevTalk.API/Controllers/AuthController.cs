using DevTalk.Application.Posts.Commands.CreatePosts;
using DevTalk.Application.ApplicationUser.Commands.RegisterUser;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DevTalk.Application.ApplicationUser.Commands.LoginUser;
using Swashbuckle.AspNetCore.Annotations;
using DevTalk.Application.ApplicationUser.Commands.CreateRefreshToken;

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
            if (!string.IsNullOrEmpty(authResponse.RefreshToken))
                SetRefreshTokenInCookie(authResponse.RefreshToken, authResponse.RefreshTokenExpiration);
            return Ok(authResponse);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AuthResponse>> LoginUser([FromBody] LoginUserCommand command)
        {
            var authResponse = await _mediator.Send(command);
            if (!authResponse.IsAuthenticated)
                return BadRequest(authResponse);
            if (!string.IsNullOrEmpty(authResponse.RefreshToken))
                SetRefreshTokenInCookie(authResponse.RefreshToken, authResponse.RefreshTokenExpiration);
            return Ok(authResponse);
        }

        [HttpGet("refreshToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AuthResponse>> CreateRefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                throw new CustomeException("Invalid token");
            var result = await _mediator.Send(new CreateRefreshTokenCommand(refreshToken));
            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
            return Ok(result);
        }

        [SwaggerIgnore]
        public void SetRefreshTokenInCookie(string refreshToken,DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
