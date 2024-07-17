using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Proto.Todo.V1;

namespace GrpcService.Services;

public class TodoService : Proto.Todo.V1.TodoService.TodoServiceBase
{
    [Authorize]
    public override Task<TodoServiceCreateResponse> Create(TodoServiceCreateRequest request, ServerCallContext context)
    {
        var user = context.GetHttpContext().User;
        var uid = user.Claims.FirstOrDefault(claim => claim.Type == "user_id")?.Value;
        var token = context.RequestHeaders.Get("Authorization");
        Console.WriteLine("==========================");
        Console.WriteLine(token?.Value ?? "No token");
        Console.WriteLine("==========================");
        return Task.FromResult(new TodoServiceCreateResponse
        {
            // Title = $"Todo: {request.Title}, User: {uid ?? "Unknown"}",
            Description = string.IsNullOrEmpty(request.Description) ? "No description" : request.Description,
            Done = false
        });
    }

    public override Task<TodoServiceDeleteResponse> Delete(TodoServiceDeleteRequest request, ServerCallContext context)
    {
        return Task.FromResult(new TodoServiceDeleteResponse
        {
            Succeded = true
        });
    }
}