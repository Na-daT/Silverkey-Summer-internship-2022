using Grpc.Core;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Text;
using Server;

namespace Server.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        var message = new StringBuilder();
        foreach (var r in request.Recipes)
        {
            message.AppendLine(r.Title);
        }
        return Task.FromResult(new HelloReply
        {
            Message = message.ToString()
        });
    }
}
