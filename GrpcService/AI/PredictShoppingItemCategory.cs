using Claudia;

namespace GrpcService.AI;

/// <summary>
///     買い物アイテムのカテゴリを予測する
/// </summary>
public class PredictShoppingItemCategory(IConfiguration config, ILogger<PredictShoppingItemCategory> logger)
{
    public enum Category
    {
        Food,　// 食料品
        Clothing, // 衣類
        Furniture, // 家具
        Electronics, // 電化製品
        Other // その他
    }

    private readonly Anthropic _anthropic = new()
    {
        ApiKey = config["AnthropicApiKey"] ?? throw new NullReferenceException("AnthropicApiKey is not set")
    };

    public async Task<Category> PredictCategory(string itemName)
    {
        try
        {
            var msg = await _anthropic.Messages.CreateAsync(new MessageRequest
            {
                Model = Claudia.Models.Claude3Haiku,
                MaxTokens = 1000,
                Temperature = 0,
                Messages =
                [
                    new Message
                    {
                        Role = "user",
                        Content = $"""
                                   Your task is to analyze a given text and classify the product names into several categories. Please follow the steps below carefully:

                                   <item_name>
                                     {itemName}
                                   </item_name>

                                   <categories>
                                   {
                                       string.Join(
                                           "\n",
                                           Enum
                                               .GetNames<Category>()
                                               .Select(
                                                   name => $"""
                                                              <category>
                                                                <value>{name}</value>
                                                              </category>
                                                            """
                                               )
                                       )
                                   }
                                   </categories>

                                   The product name is given in Japanese. The product name is enclosed in the <item_name> tag. The categories to be classified are in an array format enclosed in the <categories> tag. Classify the product names enclosed in the <item_name> tag into the categories in <categories>. Output only the category name. No other information is required. Also, do not use category names that do not exist in <categories>.
                                   """
                    }
                ]
            });

            return Enum.TryParse(msg.ToString(), true, out Category category)
                ? category
                : Category.Other;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to predict category");
            return Category.Other;
        }
    }
}
