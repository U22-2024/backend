using Grpc.Net.Client;
using Remind.V1;
using Xunit.Abstractions;

namespace GrpcServiceTest;

public class IntegrationTest1(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public async Task GetWebResourceRootReturnsOkStatusCode()
    {
        // Arrange
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AppHost>();
        // To output logs to the xUnit.net ITestOutputHelper, consider adding a package from https://www.nuget.org/packages?q=xunit+logging

        await using var app = await appHost.BuildAsync();
        var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
        await app.StartAsync();

        // Act
        await resourceNotificationService.WaitForResourceAsync("grpc", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));
        var channel = GrpcChannel.ForAddress("http://grpc:5065");
        var stub = new RemindGroupService.RemindGroupServiceClient(channel);

        var request = new GetRemindGroupRequest
        {
            Id = Guid.NewGuid().ToString()
        };
        var response = await stub.GetRemindGroupAsync(request);
        testOutputHelper.WriteLine(response.RemindGroup.Title);

        // Assert
    }
}
