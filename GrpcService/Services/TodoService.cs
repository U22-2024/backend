using Grpc.Core;
using Proto.Todo.V1;

namespace GrpcService.Services;

public class TodoService : Proto.Todo.V1.TodoService.TodoServiceBase
{
    public override Task<TodoServiceCreateResponse> Create(TodoServiceCreateRequest request, ServerCallContext context)
    {
        return Task.FromResult(new TodoServiceCreateResponse
        {
            Title = "Hello World!",
            Description = "This is a new todo item.",
            Done = true
        });
    }
}