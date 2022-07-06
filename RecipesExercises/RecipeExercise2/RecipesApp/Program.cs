using Microsoft.Extensions.Configuration;

namespace RecipesApp;
class Program
{
    static async Task Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        var app = new ConsoleApp(builder.Build());
        await app.RunMain();
    }
}

