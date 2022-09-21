using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RecipeExercise9;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var baseAddress = builder.Configuration["BaseAddress"] ?? builder.HostEnvironment.BaseAddress;
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7047/api/json/") });

builder.Services.AddSingleton<IRecipeService, RecipeService>();
builder.Services.AddSingleton<ICategoryService, CategoryService>();
builder.Services.AddSingleton<IUserService, UserService>();

await builder.Build().RunAsync();
