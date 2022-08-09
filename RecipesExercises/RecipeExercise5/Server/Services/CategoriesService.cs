using Grpc.Core;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Text;
using Server;
using Google.Protobuf;

namespace Server.Services;

public class CategoriesService : Categories.CategoriesBase
{
    private List<Category> _categories = new List<Category>();
    //private JsonFormatter jsonFormatter = new JsonFormatter(new JsonFormatter.Settings(false));
    private readonly ILogger<CategoriesService> _logger;
    
    public CategoriesService(ILogger<CategoriesService> logger)
    {
        _logger = logger;
        this._categories = Utility.LoadCategories();
    }

    public override async Task ListCategories(ListCategoriesRequest request, IServerStreamWriter<Category> responseStream, ServerCallContext context)
    {
        var responses = _categories;
        foreach(var response in responses)
        {
           await responseStream.WriteAsync(response);
        }
    }

    public override Task<AddCategoryResponse> AddCategory(AddCategoryRequest request, ServerCallContext context)
    {
        Category newCategory = new Category();
        newCategory.Name = request.Title;
        newCategory.Id = Guid.NewGuid().ToString();
        _categories.Add(newCategory);
        Utility.SaveCategories(_categories);
        return Task.FromResult( new AddCategoryResponse { Name = newCategory.Name });
    }

    public override Task<UpdateCategoryResponse> UpdateCategory(UpdateCategoryRequest request, ServerCallContext context)
    {
        var UpdatedCategory = _categories.FirstOrDefault(x => x.Id == request.Id);
        if (UpdatedCategory != null)
        {
            UpdatedCategory.Name = request.Name;
            Utility.SaveCategories(_categories);
            return Task.FromResult(new UpdateCategoryResponse { ResultMessage = "success" });
        }
        return Task.FromResult(new UpdateCategoryResponse { ResultMessage = "fail" });
    }
}