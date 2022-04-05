using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebClient.Pages;

public class ProductModel : PageModel
{
    private readonly ICmmrcApi _api;
    private readonly ILogger<ProductModel> _logger;

    public ProductModel(
        ICmmrcApi api,
        ILogger<ProductModel> logger)
    {
        _api = api;
        _logger = logger;
    }

    [FromQuery(Name = "id")]
    public string Id { get; set; }

    public async Task OnGet()
    {
        ViewData["Product"] = await _api.GetCatalogItem(Id);
    }
}
