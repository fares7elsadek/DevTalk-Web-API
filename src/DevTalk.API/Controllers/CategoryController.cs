using DevTalk.Application.Category.Commands.CreateCategory;
using DevTalk.Application.Category.Commands.DeleteCategory;
using DevTalk.Application.Category.Queries.GetAllCategories;
using DevTalk.Application.Category.Queries.GetCategoryById;
using DevTalk.Application.Category.Queries.GetCategoryPosts;
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
        public async Task<ActionResult<ApiResponse>> GetAllCategories(
            [FromQuery] int page = 1, [FromQuery] int size = 5)
        {
            if (page < 0) page = 1;
            var categories = await _mediator.Send(new GetAllCategoriesQuery());
            int total = categories.Count();
            if (size > total) size = 5;
            int pages = (int)Math.Ceiling((decimal)total / size);
            if (page > pages)
            {
                page = pages;
            }
            var Result = categories.Skip((page - 1) * size).Take(size).ToList();
            apiResponse.IsSuccess = true;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.Result = Result;
            return Ok(apiResponse);
        }

        [HttpGet("{categoryId}/posts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetAllCategoryPosts(
            [FromRoute] string categoryId,[FromQuery] int page = 1, [FromQuery] int size = 5)
        {
            if (page < 0) page = 1;
            var Posts = await _mediator.Send(new GetCategoryPostsQuery(categoryId));
            int total = Posts.Count();
            if (size > total) size = 5;
            int pages = (int)Math.Ceiling((decimal)total / size);
            if (page > pages)
            {
                page = pages;
            }
            var Result = Posts.Skip((page - 1) * size).Take(size).ToList();
            apiResponse.IsSuccess = true;
            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.Result = Result;
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
