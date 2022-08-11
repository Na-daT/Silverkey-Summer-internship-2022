using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Grpc.Net.Client;
using Grpc.Core;

namespace Client_Razor;
public class CategoriesEditModel : PageModel
{
    private readonly ILogger<CategoriesEditModel> _logger;

    [BindProperty]
    public Category UpdatedCategory { get; set; } = new();

    public CategoriesEditModel(ILogger<CategoriesEditModel> logger)
    {
        _logger = logger;
    }

    public async Task OnGetAsync(string id)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7103");
            var client = new Categories.CategoriesClient(channel);
            var reply = client.ListCategories(new ListCategoriesRequest());
            await foreach (var category in reply.ResponseStream.ReadAllAsync())
            {
                if (category.Id.ToString() == id)
                {
                    UpdatedCategory = category;
                }
            }
            if (UpdatedCategory == null)
            {
                throw new Exception();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while calling the gRPC service.");
        }
    }

    public async Task<IActionResult> OnPostAsync(string id)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7103");
            var client = new Categories.CategoriesClient(channel);
            var request = new UpdateCategoryRequest { Id = id, Name = UpdatedCategory.Name };
            var reply = await client.UpdateCategoryAsync(request);
            if (reply.ResultMessage.ToString() == "fail")
            {
                throw new Exception();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while calling the gRPC service.");
        }
        return RedirectToPage("/Categories/List");
    }
}