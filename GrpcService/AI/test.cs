using System;
using Microsoft.Extensions.Configuration;

public class test
{
    public static void Test()
    {
        var cbr = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

        Console.WriteLine($"secret api key is {cbr["AnthropicApiKey"]}");
    }
}
