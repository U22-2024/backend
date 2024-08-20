using Claudia;
using System.Text.Json;

public class PredictRemindType(IConfiguration _config)
{
    private Anthropic _anthropic = new Anthropic
    {
        ApiKey = _config["AnthropicApiKey"]
    };

    /// <summary>
    ///  Get remind type from the given prompt.
    ///  This method uses the Anthropic API to generate a reminder type from the given prompt
    /// </summary>
    /// <param name="prompt"></param>
    /// <param name="categories"></param>
    /// <returns></returns>
    public async Task<RemindTypeResponse> GetRemindType(string prompt, string[] categories)
    {
        // if use it :
        // PredictRemindType predictRemindType;
        // using (var scope = context.GetHttpContext().RequestServices.CreateScope())
        // {
        //     predictRemindType = scope.ServiceProvider.GetRequiredService<PredictRemindType>();
        //     predictRemindType.GetRemindType("トマトを買う", shoppingItems);
        // }

        var message = await _anthropic.Messages.CreateAsync(new()
        {
            Model = "claude-3-5-sonnet-20240620",
            MaxTokens = 1000,
            Temperature = 0,
            Messages = [new ()
                {
                    Role = "user",
                    Content = $"You are an AI assistant tasked with analyzing a given sentence and categorizing it based on a provided list of categories. You will also extract information about what item is being purchased (if applicable) and its quantity. Follow these steps:\n\n1. You will be given a list of categories in the following format:\n<categories>\n{string.Join(", ", categories)}\n</categories>\n\n2. You will then be presented with a sentence to analyze:\n<sentence>\n{prompt}\n</sentence>\n\n3. Your task is to determine the category of the sentence, identify the item being purchased (if any), and specify the quantity. You will provide this information in a JSON format.\n\n4. To classify the sentence:\n   - If the sentence is about buying items typically found in a supermarket, classify it as \"スーパー\".\n   - If the sentence is about buying items not typically found in a supermarket but available in large shopping malls, classify it as \"他買い物\".\n   - If the sentence is not about purchasing anything, classify it into one of the other categories provided.\n\n5. If the sentence is about purchasing an item:\n   - Identify the item being purchased.\n   - Determine the quantity of the item, if specified.\n   If is not:\n   - The item should be the sentence itself\n\n6. Provide your output in the following JSON format:\n   {{\n     \"type\":\"category\",\n     \"name\":\"item name\",\n     \"quantity\":\"quantity (if applicable)\"\n   }}\n\n   If the sentence is not about purchasing an item, the \"quantity\" field in JSON output should be empty string.\n\nRemember to think carefully about the classification and extraction of information before providing your final answer. Output your response only JSON format."
                }
            ]
        });

        Console.WriteLine(message.ToString());

        RemindTypeResponse? response = JsonSerializer.Deserialize<RemindTypeResponse>(message.ToString());

        Console.WriteLine($"response.type = {response.type}");

        return response;
    }
}

public class RemindTypeResponse
{
    public string type;
    public string name;
    public string quantity;
}
