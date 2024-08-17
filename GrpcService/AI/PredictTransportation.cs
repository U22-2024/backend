using Claudia;

namespace GrpcService.ClaudeAI;

public class PredictTransportation
{
    private Anthropic anthropic = new Anthropic
    {
        ApiKey = Environment.GetEnvironmentVariable("ANTHROPIC_API_KEY")
    };

    public async void GetTransportation(string prompt)
    {
        var message = await anthropic.Messages.CreateAsync(new()
        {
            Model = "claude-3-5-sonnet-20240620",
            MaxTokens = 1000,
            Temperature = 0,
            Messages = [new ()
                {
                    Role = "user",
                    Content = @"You are tasked with determining whether public transportation (bus, train, and plane) is necessary for a given event. You will receive an event description and must decide if each mode of transportation is required.Here is the event description:
<event>
{{EVENT}}
</event>

To make your decision, consider the following factors:
1. Distance: Is the event location far enough to require air travel?
2. Accessibility: Is the event in an urban area where buses or trains are common?
3. Event type: Does the nature of the event suggest a need for specific transportation?

You should output your decision in JSON format with the following structure:
{
  ""bus"": boolean,
  ""train"": boolean,
  ""plane"": boolean
}

Where each boolean value (true or false) indicates whether that mode of transportation is necessary for the event.

For example, if an event requires both bus and train transportation but not air travel, the output would be:
{
  ""bus"": true,
  ""train"": true,
  ""plane"": false
}

Please provide only your final answer in JSON format."
                }
            ]
        });

        Console.WriteLine(message);
    }
}
