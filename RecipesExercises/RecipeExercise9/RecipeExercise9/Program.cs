using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CurrieTechnologies.Razor.SweetAlert2;
using RecipeExercise9;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var baseAddress = builder.Configuration.GetValue<string>("BaseAdress");
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });

builder.Services.AddSingleton<IRecipeService, RecipeService>();
builder.Services.AddSingleton<ICategoryService, CategoryService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSweetAlert2();

await builder.Build().RunAsync();
