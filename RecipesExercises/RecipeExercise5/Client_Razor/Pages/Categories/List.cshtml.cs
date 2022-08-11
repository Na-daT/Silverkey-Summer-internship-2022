using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Grpc.Net.Client;
using Grpc.Core;

namespace Client_Razor;
public class CategoriesModel : PageModel
{
    private readonly ILogger<CategoriesModel> _logger;

    [BindProperty]
    public List<Category> ListCategories { get; set; } = new();

    public CategoriesModel(ILogger<CategoriesModel> logger)
    {
        _logger = logger;
    }

    public async Task OnGetAsync()
    {
        try
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7103");
            var client = new Categories.CategoriesClient(channel);
            var reply = client.ListCategories(new ListCategoriesRequest());
            await foreach (var category in reply.ResponseStream.ReadAllAsync())
            {
                ListCategories.Add(category);
            }
            ListCategories = ListCategories.OrderBy(x => x.Name).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while calling the gRPC service.");
        }
    }
}