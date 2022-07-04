using System.Net.Http;
using System.Text.Json;
using System.Web.Http;
using System.Text;
using System.Net.Http.Json;

namespace RecipesApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var app = new ConsoleApp();
            await app.RunMain();
        }
    }
}
