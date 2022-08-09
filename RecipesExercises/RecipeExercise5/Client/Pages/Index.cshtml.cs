using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using Grpc.Net.Client;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace Client.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<Category> _categories = new();

        [BindProperty]
        public string CategoryTitle { get; set; }

        [BindProperty]
        public string UpdatedName{ get; set; }
        
        public IndexModel(ILogger<IndexModel> logger)
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
                    _categories.Add(category);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calling the gRPC service.");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                using var channel = GrpcChannel.ForAddress("https://localhost:7103");
                var client = new Categories.CategoriesClient(channel);
                var request = new Client.AddCategoryRequest { Title = CategoryTitle };
                var reply = await client.AddCategoryAsync(request);
                if (reply.Name.ToString() == String.Empty)
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calling the gRPC service.");
            }
            return RedirectToPage("/Index");
        }
        
        public async Task<IActionResult> OnPostUpdateAsync(string id)
        {
            try
            {
                using var channel = GrpcChannel.ForAddress("https://localhost:7103");
                var client = new Categories.CategoriesClient(channel);
                var request = new UpdateCategoryRequest { Id = id, Name = UpdatedName };
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
            return RedirectToPage("/Index");
        }
    }
}