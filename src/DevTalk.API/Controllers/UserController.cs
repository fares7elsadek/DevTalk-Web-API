using DevTalk.Application.ApplicationUser.Commands.UploadAvatar;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DevTalk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private ApiResponse apiResponse;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
            apiResponse = new ApiResponse();
        }

        [HttpPost("avatar")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> AddAvatar([FromForm] UploadAvatarCommand command)
        {
            var url = await _mediator.Send(command);
            apiResponse.IsSuccess = true;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.Result = url;
            return Ok(apiResponse);
        }
    }
}
