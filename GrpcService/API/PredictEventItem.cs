using Claudia;
using Grpc.Core;

namespace GrpcService.API;

public class PredictEventItem(IConfiguration config)
{
    private readonly Anthropic _anthropic = new Anthropic
    {
        ApiKey = config["AnthropicApiKey"] ?? throw new Exception("AnthropicApiKey is not set.")
    };

    public async Task<string[]> PredictEventItemList(string text)
    {
        string[] responseInfo = [];
        try
        {
            var message = await _anthropic.Messages.CreateAsync(new MessageRequest
            {
                Model = "claude-3-5-sonnet-20240620",
                MaxTokens = 1000,
                Temperature = 0,
                Messages =
                [
                    new Message
                    {
                        Role = "user",
                        Content =
                            $"You are tasked with breaking down a given Japanese text into separate tasks and outputting them in a specific format. Here's how to approach this:\n\n1. You will be given a text input in Japanese. The text will be provided within <input_text> tags:\n\n<input_text>\n{text}\n</input_text>\n\n2. Read through the text carefully and identify distinct tasks or items mentioned.\n\n3. For each task or item you identify:\n   - If it's an item to bring or carry, prefix it with \"持ち物: \" (items to bring)\n   - For other tasks, simply state the task as is\n\n4. Separate each identified task or item with a comma (,) without spaces.\n\n5. If there are no tasks or items to list, output \"None\".\n\nHere are some examples to guide you:\n\nExample 1:\nInput: \"明日の会議の資料を準備して、山田さんにメールを送る必要がある。\"\nOutput: 明日の会議の資料を準備する,山田さんにメールを送る\n\nExample 2:\nInput: \"出かける前に傘と財布を忘れずに。あと、スーパーで牛乳を買わなきゃ。\"\nOutput: 持ち物: 傘,持ち物: 財布,スーパーで牛乳を買う\n\nExample 3:\nInput: \"今日はゆっくり休もう。\"\nOutput: None\n\nNow, process the given input text and provide your output on a single line, with tasks separated by commas as described above. Do not include any explanation or additional text in your response."
                    }
                ]
            });

            if (responseInfo.ToString() != "None")
                responseInfo = message.ToString().Split(",");
            if (responseInfo == null)
                throw new RpcException(new Status(StatusCode.Internal, "Claude API error"));
        }
        catch (ClaudiaException ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, $"Claude API error | {ex.Status} | {ex.Name} | {ex.Message}"));
        }

        return responseInfo;
    }
}
