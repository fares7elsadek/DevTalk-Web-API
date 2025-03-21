using DevTalk.Application.Notification.Commands.MarkAllAsRead;
using DevTalk.Application.Notification.Commands.MarkAsRead;
using DevTalk.Application.Notification.Queries.GetAllNotifications;
using DevTalk.Domain.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DevTalk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private ApiResponse apiResponse;
        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
            apiResponse = new ApiResponse();
        }

        [HttpGet("all")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetAllNotifications([FromQuery]string cursor = "",
            int pageSize = 5)
        {
            var notifications = await _mediator.Send(new GetAllNotificationsQuery(cursor, pageSize));
            apiResponse.IsSuccess = true;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.Result = notifications;
            return Ok(apiResponse);
        }

        [HttpPost("{notificationId}/read")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> MarkAsRead([FromRoute]string notificationId)
        {
            await _mediator.Send(new MarkAsReadCommand(notificationId));
            apiResponse.IsSuccess = true;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.Result = null;
            return Ok(apiResponse);
        }

        [HttpPost("read")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> MarkAllAsRead()
        {
            await _mediator.Send(new MarkAllAsReadCommand());
            apiResponse.IsSuccess = true;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.Result = null;
            return Ok(apiResponse);
        }
    }
}
