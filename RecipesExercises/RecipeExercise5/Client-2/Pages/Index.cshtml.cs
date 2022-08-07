using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Grpc.Net.Client;
using Grpc.Core;
using System.Threading.Tasks;

namespace Client_2.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7103");
            var client = new Greeter.GreeterClient(channel);
            var request = new Greeter.GreeterClient.HelloRequest();
            request.Recipes.Add(new Recipe { Title = "Recipe 1", Ingredients = new[] { "Ingredient 1", "Ingredient 2" }, Instructions = new[] { "Instruction 1", "Instruction 2" }, Categories = new[] { new Category { Id = "1", Name = "Category 1" } } });
            request.Categories.Add(new Category { Id = "1", Name = "Category 1" });

            var reply = await client.SayHello(request);
            _logger.LogInformation(reply.Message);
        }
    }
}