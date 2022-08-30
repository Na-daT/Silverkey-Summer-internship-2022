using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Antiforgery;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter JWT Bearer token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {securityScheme, new string[] { }}
    });
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
     .AddJwtBearer(c =>
     {
         c.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = true,
             ValidateAudience = true,
             ValidateLifetime = true,
             ValidateIssuerSigningKey = true,
             ValidAudience = builder.Configuration["JWT:Audience"],
             ValidIssuer = $"{builder.Configuration["JWT:Issuer"]}",
             IssuerSigningKey = new SymmetricSecurityKey
               (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
         };
     });

builder.Services.AddAuthorization();
builder.Logging.SetMinimumLevel(LogLevel.Warning);
builder.Logging.AddConsole();
builder.Services.AddTransient<ITokenService, TokenService>();


var app = builder.Build();
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = String.Empty;
});
var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true,
};

app.MapPost("api/json/login", [AllowAnonymous] async ([FromBody] LoginModel loginModel) =>
{
    var hasher = new PasswordHasher<LoginModel>();
    TokenService _tokenService = new TokenService();
    var usersJson = await FileHandler.ReadAsync("users.json");
    var usersList = await JsonHandler.DeserializeAsync<List<LoginModel>>(usersJson);
    if (usersList is null)
        throw new Exception("Could not deserialize users list");
    var user = usersList.FirstOrDefault(u => u.UserName == loginModel.UserName);
    if (user is null)
        return Results.Unauthorized();
    var isPasswordMatch = hasher.VerifyHashedPassword(new LoginModel(), user.Password, loginModel.Password);
    if (isPasswordMatch == PasswordVerificationResult.Failed)
        return Results.Unauthorized();

    var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, loginModel.UserName)
        };
    var accessToken = _tokenService.GenerateAccessToken(claims);
    var refreshToken = _tokenService.GenerateRefreshToken();

    user.RefreshToken = refreshToken;
    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7).ToString();


    var json = await JsonHandler.SerializeAsync(usersList);
    await FileHandler.WriteAsync("users.json", json);

    return Results.Ok(new AuthenticatedResponse { RefreshToken = refreshToken, Token = accessToken });
});

app.MapPost("api/json/register", async ([FromBody] RegisterModel newUser) =>
{
    var hasher = new PasswordHasher<LoginModel>();
    var usersJson = await FileHandler.ReadAsync("users.json");
    var usersList = await JsonHandler.DeserializeAsync<List<LoginModel>>(usersJson);
    if (usersList is null)
        throw new Exception("Could not deserialize users list");
    if (usersList.Find(x => x.UserName == newUser.Username) != null)
        return Results.BadRequest("username already exists");
    var hashedPassword = hasher.HashPassword(new LoginModel(), newUser.Password);
    LoginModel newLoginModel = new LoginModel(Guid.NewGuid().ToString(), newUser.Username, hashedPassword, string.Empty, string.Empty);
    usersList.Add(newLoginModel);
    var json = await JsonHandler.SerializeAsync(usersList);
    await FileHandler.WriteAsync("users.json", json);

    return Results.Ok();
});

app.MapPost("api/json/refresh-token", async ([FromBody] RefreshRequest request) =>
{
    if (request is null)
        return Results.BadRequest("Invalid client request");

    string accessToken = request.Token;
    string refreshToken = request.RefreshToken;
    TokenService _tokenService = new TokenService();


    //get principal from expired token
    var principal = _tokenService.ValidateExpiredToken(accessToken);

    //get username from claim
    var username = principal.Identity.Name;

    //deserialize and fetch user from json db
    var usersJson = await FileHandler.ReadAsync("users.json");
    var usersList = await JsonHandler.DeserializeAsync<List<LoginModel>>(usersJson);
    if (usersList is null)
        throw new Exception("Could not deserialize users list");
    var user = usersList.FirstOrDefault(u => u.UserName == username);

    //check if user exists, if refrsh tokens are the same and if refresh token is not expired yet.
    if (user is null || user.RefreshToken != refreshToken || DateTime.Parse(user.RefreshTokenExpiryTime) <= DateTime.Now)
        return Results.BadRequest("Invalid client request");

    //create new tokens
    var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
    var newRefreshToken = _tokenService.GenerateRefreshToken();

    //update refresh token of user and save json file
    user.RefreshToken = newRefreshToken;
    var json = await JsonHandler.SerializeAsync(usersList);
    await FileHandler.WriteAsync("users.json", json);

    //return new auth token and refresh token
    return Results.Ok(new AuthenticatedResponse()
    {
        Token = newAccessToken,
        RefreshToken = newRefreshToken
    });

});

app.MapPost("api/json/revoke-token", [Authorize] async ([FromBody]string token) =>
{
    if (string.IsNullOrEmpty(token))
        return Results.BadRequest(new { message = "Token is required" });
    var usersJson = await FileHandler.ReadAsync("users.json");
    var usersList = await JsonHandler.DeserializeAsync<List<LoginModel>>(usersJson);
    if (usersList is null)
        throw new Exception("Could not deserialize users list");
    var user = usersList.FirstOrDefault(u => u.RefreshToken == token);
    if (user == null)
        return Results.BadRequest();
    user.RefreshToken = null;
    var json = await JsonHandler.SerializeAsync(usersList);
    await FileHandler.WriteAsync("users.json", json);
    return Results.Ok();
});

app.MapGet("/antiforgery", (IAntiforgery antiforgery, HttpContext context) =>
{
    var tokens = antiforgery.GetAndStoreTokens(context);
    context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken!, new CookieOptions { HttpOnly = false });
});

app.MapPost("/validate", async (HttpContext context, IAntiforgery antiforgery) =>
{
    try
    {
        await antiforgery.ValidateRequestAsync(context);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.Problem(ex?.Message ?? string.Empty);
    }
});

app.MapGet("api/json/{fileName}", [Authorize] async Task<string> (string fileName) =>
{
    var jsonFile = fileName + ".json";
    return await FileHandler.ReadAsync(jsonFile);
}).RequireAuthorization();

app.MapPost("api/json/recipes", [Authorize] async ([FromBody] Recipe recipeToPost) =>
{
    try
    {
        var recipes = await FileHandler.ReadAsync("recipe.json");
        var recipesList = await JsonHandler.DeserializeAsync<List<Recipe>>(recipes);
        if (recipesList is null)
            throw new Exception("Could not deserialize recipes list");
        recipesList.Add(recipeToPost);
        var json = await JsonHandler.SerializeAsync(recipesList);
        await FileHandler.WriteAsync("recipe.json", json);
        return Results.Ok();
    }
    catch (Exception e)
    {
        app.Logger.LogError(e.Message);
        return Results.StatusCode(500);
    }
});

app.MapPut("api/json/recipes", [Authorize] async ([FromBody] Recipe recipeToUpdate) =>
{
    try
    {
        var recipes = await FileHandler.ReadAsync("recipe.json");
        var recipesList = await JsonHandler.DeserializeAsync<List<Recipe>>(recipes);
        if (recipesList is null)
            throw new Exception("Could not deserialize recipes list");
        var recipe = recipesList.FirstOrDefault(x => x.Id == recipeToUpdate.Id);
        if (recipe is null)
            return Results.StatusCode(404);
        recipesList[recipesList.IndexOf(recipe)] = recipeToUpdate;
        var json = await JsonHandler.SerializeAsync(recipesList);
        await FileHandler.WriteAsync("recipe.json", json);
        return Results.Ok();
    }
    catch (Exception e)
    {
        app.Logger.LogError(e.Message);
        return Results.StatusCode(500);
    }
});

app.MapDelete("api/json/recipes/{id}", [Authorize] async (Guid id) =>
{
    try
    {
        var recipes = await FileHandler.ReadAsync("recipe.json");
        var recipesList = await JsonHandler.DeserializeAsync<List<Recipe>>(recipes);
        if (recipesList is null)
            throw new Exception("Could not deserialize recipes list");
        var recipe = recipesList.FirstOrDefault(x => x.Id == id);
        if (recipe is null)
            return Results.StatusCode(404);
        recipesList.Remove(recipe);
        var json = await JsonHandler.SerializeAsync(recipesList);
        await FileHandler.WriteAsync("recipe.json", json);
        return Results.Ok();
    }
    catch (Exception e)
    {
        app.Logger.LogError(e.Message);
        return Results.StatusCode(500);
    }
});

app.MapPost("api/json/categories", [Authorize] async ([FromBody] Category categoryToPost) =>
{
    try
    {
        var categories = await FileHandler.ReadAsync("category.json");
        var categoriesList = await JsonHandler.DeserializeAsync<List<Category>>(categories);
        if (categoriesList is null)
            throw new Exception("Could not deserialize categories list");
        categoriesList.Add(categoryToPost);
        var json = await JsonHandler.SerializeAsync(categoriesList);
        await FileHandler.WriteAsync("category.json", json);
        return Results.Ok();
    }
    catch (Exception e)
    {
        app.Logger.LogError(e.Message);
        return Results.StatusCode(500);
    }
});

app.MapPut("api/json/categories", [Authorize] async ([FromBody] Category categoryToUpdate) =>
{
    try
    {
        var categories = await FileHandler.ReadAsync("category.json");
        var categoriesList = await JsonHandler.DeserializeAsync<List<Category>>(categories);
        if (categoriesList is null)
            throw new Exception("Could not deserialize categories list");
        var category = categoriesList.FirstOrDefault(x => x.Id == categoryToUpdate.Id);
        if (category is null)
            return Results.StatusCode(404);
        category.Name = categoryToUpdate.Name;
        var json = await JsonHandler.SerializeAsync(categoriesList);
        await FileHandler.WriteAsync("category.json", json);
        return Results.Ok();
    }
    catch (Exception e)
    {
        app.Logger.LogError(e.Message);
        return Results.StatusCode(500);
    }
});

app.Run();