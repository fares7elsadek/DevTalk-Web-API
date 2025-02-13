using DevTalk.Application.Category.Commands.CreateCategory;
using DevTalk.Application.Category.Commands.DeleteCategory;
using DevTalk.Application.Category.Queries.GetAllCategories;
using DevTalk.Application.Category.Queries.GetCategoryById;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DevTalk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private ApiResponse apiResponse;
        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
            apiResponse = new ApiResponse();
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetAllCategories()
        {
            var categories = await _mediator.Send(new GetAllCategoriesQuery());
            apiResponse.IsSuccess = true;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.Result = categories;
            return Ok(apiResponse);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetCategory(
            [FromRoute] string categoryId)
        {
            var category = await _mediator.Send(new GetCategoryByIdQuery(categoryId));
            apiResponse.IsSuccess = true;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.Result = category;
            return Ok(apiResponse);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateCategory(CreateCategoryCommand command)
        {
            await _mediator.Send(command);
            return Created();
        }

        [HttpDelete("{categoryId}")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteCategory(string categoryId)
        {
            await _mediator.Send(new DeleteCategoryCommand(categoryId));
            return Ok();
        }
    }
}
