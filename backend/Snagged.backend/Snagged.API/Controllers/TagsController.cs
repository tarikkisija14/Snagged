using MediatR;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Catalog.Tags.Queries.GetPopularTags;
using Snagged.Application.Catalog.Tags.Queries.GetTagSuggestions;

namespace Snagged.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController(IMediator mediator) : ControllerBase
    {
        [HttpGet("suggestions")]
        public async Task<IActionResult> GetSuggestions([FromQuery] GetTagSuggestionsQuery query)
        {
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopular([FromQuery] GetPopularTagsQuery query)
        {
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}