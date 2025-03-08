using DevTalk.Application.ApplicationUser.Commands.ConfirmEmail;
using DevTalk.Application.ApplicationUser.Commands.CreateRefreshToken;
using DevTalk.Application.ApplicationUser.Commands.ForgotPassword;
using DevTalk.Application.ApplicationUser.Commands.GoogleSingInCallback;
using DevTalk.Application.ApplicationUser.Commands.LoginUser;
using DevTalk.Application.ApplicationUser.Commands.RegisterUser;
using DevTalk.Application.ApplicationUser.Commands.ResendConfirmationLink;
using DevTalk.Application.ApplicationUser.Commands.ResetPassword;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Helpers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DevTalk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private ApiResponse apiResponse;
        private SignInManager<User> _signInManager;
        public AuthController(IMediator mediator, SignInManager<User> signInManager)
        {
            _mediator = mediator;
            apiResponse = new ApiResponse();
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterUserCommand command)
        {
            var authResponse = await _mediator.Send(command);
            
            var response = new
            {
                message = authResponse.Message,
                email = authResponse.Email,
            };

            apiResponse.IsSuccess = true;
            apiResponse.Errors = null;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.Result = response;
            return Ok(apiResponse);
        }

        [HttpGet("confirmEmail", Name = "ConfirmEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> ConfirmEmail([FromQuery] string UserId, [FromQuery] string Code)
        {
            var response = await _mediator.Send(new ConfirmEmailCommand(UserId, Code));
            apiResponse.IsSuccess = true;
            apiResponse.Errors = null;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.Result = response;
            return Ok(apiResponse);
        }

        [HttpPost("resendConfirmationEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> ResendConfirmationEmail([FromBody]ResendConfirmationLinkCommand command)
        {
            var response = await _mediator.Send(command);
            apiResponse.IsSuccess = true;
            apiResponse.Errors = null;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.Result = response;
            return Ok(apiResponse);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> LoginUser([FromBody] LoginUserCommand command)
        {
            var authResponse = await _mediator.Send(command);
            if (!authResponse.IsAuthenticated)
                return BadRequest(authResponse);
            if (!string.IsNullOrEmpty(authResponse.RefreshToken))
                SetRefreshTokenInCookie(authResponse.RefreshToken, authResponse.RefreshTokenExpiration);

            apiResponse.IsSuccess = true;
            apiResponse.Errors = null;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.Result = authResponse;
            return Ok(apiResponse);
        }

        [HttpPost("forgetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ForgetPassword([FromBody] ForgotPasswordCommand command)
        {
            var response = await _mediator.Send(command);

            apiResponse.IsSuccess = true;
            apiResponse.Errors = null;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.Result = response;
            return Ok(apiResponse);
        }

        [HttpPost("{UserId}/{Token}/reset-password", Name = "reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordCommand command, [FromRoute] string UserId,
            [FromRoute] string Token)
        {
            var newCommand = new ResetPasswordCommand
            {
                UserId = UserId,
                Token = Token,
                NewPassword = command.NewPassword
            };
            var response = await _mediator.Send(newCommand);
            if (response == "Your password has been successfully reset.")
            {
                apiResponse.IsSuccess = true;
                apiResponse.Errors = null;
                apiResponse.StatusCode = HttpStatusCode.OK;
                apiResponse.Result = response;
                return Ok(apiResponse);
            }
            apiResponse.IsSuccess = false;
            apiResponse.Errors = null;
            apiResponse.StatusCode = HttpStatusCode.BadRequest;
            apiResponse.Result = response;
            return BadRequest(apiResponse);
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

        [HttpGet("signin/google")]
        [ProducesResponseType(StatusCodes.Status307TemporaryRedirect)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GoogleSignIn(string returnUrl = null)
        {
            var redirectUrl = Url.Action("SignInGoogleCallback", "Auth", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return Challenge(properties, "Google");
        }

        [HttpGet("google-callback")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SignInGoogleCallback(string returnUrl = null, string remoteError = null)
        {
            var command = new GoogleSingInCallbackCommand(remoteError);
            var authResponse = await _mediator.Send(command);
            if (!authResponse.IsAuthenticated)
                return BadRequest(authResponse);
            if (!string.IsNullOrEmpty(authResponse.RefreshToken))
                SetRefreshTokenInCookie(authResponse.RefreshToken, authResponse.RefreshTokenExpiration);

            var script = $@"
                  <html>
                    <body>
                      <script>
                        window.opener.postMessage({{
                          token: '{authResponse.Token}',
                          email: '{authResponse.Email}',
                          username: '{authResponse.Username}'
                        }}, 'https://dev-talk-phi.vercel.app');
                        window.close();
                      </script>
                    </body>
                  </html>";

            return Content(script, "text/html");
        }



        [SwaggerIgnore]
        public void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires,
                SameSite = SameSiteMode.None,
                Secure = true,
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
