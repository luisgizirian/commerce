using WebApi.Models;
using WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogController : ControllerBase
{
    private readonly ICatalogService _catalogService;
     private readonly ILogger<CatalogController> _logger;

    public CatalogController(
        ICatalogService catalogService,
        ILogger<CatalogController> logger) {
        _catalogService = catalogService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<List<CatalogItem>> Get(bool includeDisabled = false) =>
        await _catalogService.GetAsync(includeDisabled);

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<CatalogItem>> Get(string id)
    {
        var book = await _catalogService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        return book;
    }

    [HttpPost]
    public async Task<IActionResult> Post(CatalogItem newBook)
    {
        await _catalogService.CreateAsync(newBook);

        return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, CatalogItem updatedBook)
    {
        var book = await _catalogService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        updatedBook.Id = book.Id;

        await _catalogService.UpdateAsync(id, updatedBook);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Deactivate(string id)
    {
        var book = await _catalogService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        await _catalogService.RemoveAsync(id);

        return NoContent();
    }
}