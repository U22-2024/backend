using Greet.V1;
using Grpc.Core;

namespace GrpcService.Services;

public class GreetService(AppDbContext dbContext) : Greet.V1.GreetService.GreetServiceBase {
    public override async Task<GetGreetResponse> GetGreet(GetGreetRequest request, ServerCallContext context)
    {
        Random rnd = new Random();

        var greeting = await dbContext.Greets.FindAsync(rnd.Next(1,dbContext.Greets.Count()+1));

        return new GetGreetResponse
        {
            Greet = new Greet.V1.Greet {
                Message = greeting!.Message
            }
        };
    }
}
