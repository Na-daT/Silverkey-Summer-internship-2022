using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Grpc.Net.Client;
using Grpc.Core;

namespace Client_Razor;

public class CategoriesAddModel : PageModel
{
    private readonly ILogger<CategoriesAddModel> _logger;

    [BindProperty]
    public Category NewCategory { get; set; } = new();

    public CategoriesAddModel(ILogger<CategoriesAddModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7103");
            var client = new Categories.CategoriesClient(channel);
            var request = new AddCategoryRequest { Title = NewCategory.Name };
            var reply = await client.AddCategoryAsync(request);
            if (reply.Name.ToString() == String.Empty)
            {
                throw new Exception();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while calling the gRPC service.");

        }
        return RedirectToPage("/Categories/List");
    }
}