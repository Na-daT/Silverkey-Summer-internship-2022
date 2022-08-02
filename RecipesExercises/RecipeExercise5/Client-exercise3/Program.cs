using CurrieTechnologies.Razor.SweetAlert2;
using FluentValidation.AspNetCore;
using FluentValidation;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
var appName = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("api")["url"];
builder.Services.AddHttpClient("recipeClient", httpClient =>
{
    httpClient.BaseAddress = new Uri(appName);
});
builder.Services.AddSweetAlert2();
builder.Services.AddFluentValidation(options =>
{
    options.RegisterValidatorsFromAssemblyContaining(typeof(Program));
    options.ImplicitlyValidateChildProperties = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
