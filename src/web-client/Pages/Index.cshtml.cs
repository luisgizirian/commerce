using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebClient.Pages;

public class IndexModel : PageModel
{
    private readonly ICmmrcApi _api;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(
        ICmmrcApi api,
        ILogger<IndexModel> logger)
    {
        _api = api;
        _logger = logger;
    }

    public async Task OnGet()
    {
        ViewData["Products"] = await _api.ListCatalogItems();
    }
}
