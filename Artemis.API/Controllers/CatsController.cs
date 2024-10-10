using Artemis.API.Services.Interfaces;
using Artemis.Model.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatsController : ControllerBase
    {
        private readonly ICatService _catService;

        public CatsController(ICatService catService)
        {
            _catService = catService;
        }

        // POST api/cats/fetch
        [HttpPost("fetch")]
        public async Task<ActionResult> FetchCats()
        {
            await _catService.HandleCatsAsync();

            return CreatedAtAction(nameof(GetCatsByTagWithPaging), null);
        }

        // GET api/cats/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCatById(int id)
        {
            if (id <= 0)
                return NotFound("Id needs to be greater than 0.");

            var cat = await _catService.GetCatByIdAsync(id);

            if (cat == null)
                return NotFound("Cat with that id does not exist.");

            var viewCat = new CatSwaggerDto
            {
                Id = cat.Id,
                Tags = cat.CatTags.Select(ct => new TagSwaggerDto
                {
                    Id = ct.Tag.Id,
                    Name = ct.Tag.Name,
                    Created = ct.Tag.Created
                }).ToList()
            };

            return Ok(viewCat);
        }

        // GET api/cats
        [HttpGet]
        public async Task<IActionResult> GetCatsByTagWithPaging(int page, int pageSize, string tagName = null)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest("Page and pageSize must be greater than zero.");

            var pagedCats = await _catService.GetCatsWithPaginationByTagNameAsync(page, pageSize, tagName);

            if (pagedCats == null || pagedCats.Count() < 1)
                return NotFound("No results found");

            return Ok(pagedCats);
        }
    }
}
