using Claudia;
using System.Text.Json;

public class PredictRemindType(IConfiguration _config)
{
    private Anthropic _anthropic = new Anthropic
    {
        ApiKey = _config["AnthropicApiKey"]
    };

    public async Task<RemindTypeResponse> GetRemindType(string prompt)
    {
        var message = await _anthropic.Messages.CreateAsync(new()
        {
            Model = "claude-3-5-sonnet-20240620",
            MaxTokens = 1000,
            Temperature = 0,
            Messages = [new ()
                {
                    Role = "user",
                    Content = $"You are tasked with analyzing a given text to predict its classification, main remind, and sub-reminds. Follow these steps carefully:\\n\\n1. You will be given a text in Japanese. The text will be provided within <text> tags:\\n\\n<text>\\n{prompt}\\n</text>\\n\\n2. Your task is to classify this text into one of the following categories:\\n   - スーパーマーケット (Supermarket)\\n   - 外出 (Going out)\\n   - 学校 (School)\\n   - その他 (Other)\\n\\n3. You need to determine:\\n   a) The classification of the text\\n   b) The main remind (主なリマインド) extracted from the text\\n   c) Any sub-reminds (サブリマインド) that should be done before or during the main remind\\n\\n4. Analyze the text carefully. Consider the context and the actions described in the text to determine the most appropriate classification.\\n\\n5. For the main remind, extract the primary action or task mentioned in the text. Content not mentioned in the main text need not be output. If there are no clear sub-reminds, leave these fields empty.\\n\\n6. In the case of a sub-reminds, consider the steps that are implied by the main reminder or that are necessary to perform the main reminder or that are done in advance.  If there are no clear sub-reminds, leave these fields empty.\\n\\n7. After your analysis, provide your output in the following JSON format:\\n{{\\n  \\\"kind\\\": \\\"[分類]\\\",\\n  \\\"remind\\\": \\\"[[主なリマインド]]\\\",\\n  \\\"subRemind\\\": {{\\n    \\\"before\\\": \\\"[[事前のサブリマインド]]\\\",\\n    \\\"doing\\\": \\\"[[必要なサブリマインド]]\\\"\\n  }}\\n}}\\n\\n8. Ensure that your JSON output is properly formatted and that all fields are included, even if some are empty.\\n\\nProvide your analysis and JSON output without any additional commentary."
                }
            ]
        });

        RemindTypeResponse? response = JsonSerializer.Deserialize<RemindTypeResponse>(message.ToString());

        return response;
    }
}

public class RemindTypeResponse
{
    public string kind { get; set; }
    public string remind { get; set; }
    public SubRemind subRemind { get; set; }
}

public class SubRemind
{
    public string before { get; set; }
    public string doing { get; set; }
}
