using DevTalk.Application.Preferences.Commands.AddManyPreferences;
using DevTalk.Application.Preferences.Commands.AddNewPreference;
using DevTalk.Application.Preferences.Commands.DeletePreference;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevTalk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreferncesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PreferncesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{categoryId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddNewPrefernce([FromRoute] string categoryId)
        {
            await _mediator.Send(new AddNewPreferenceCommand(categoryId));
            return Created();
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddManyPrefernce(AddManyPreferencesCommand command)
        {
            await _mediator.Send(command);
            return Created();
        }

        [HttpDelete("{categoryId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeletePrefernce(string categoryId)
        {
            await _mediator.Send(new DeletePreferenceCommand(categoryId));
            return Ok();
        }
    }
}
