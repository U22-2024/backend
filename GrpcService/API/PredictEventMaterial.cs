﻿using System.Text.Json;
using Claudia;
using Event.V1;
using Grpc.Core;
using DateTime = Event.V1.DateTime;

namespace GrpcService.API;

public class PredictEventMaterial(IConfiguration config, ILogger<PredictEventMaterial> logger)
{
    private readonly Anthropic _anthropic = new()
    {
        ApiKey = config["AnthropicApiKey"] ?? throw new Exception("AnthropicApiKey is not set.")
    };

    public async Task<EventMaterial> UpdateEventMaterial(string prompt, EventMaterial? eventMaterial)
    {
        var startTimeStr = GetDateTimeString(eventMaterial?.StartTime);
        var endTimeStr = GetDateTimeString(eventMaterial?.EndTime);

        var knownInfo = new ClaudeFormat(eventMaterial?.IsOut ?? false, eventMaterial?.Remind ?? "",
            eventMaterial?.Destination ?? "",
            (int)(eventMaterial?.MoveType ?? MoveType.Other), startTimeStr, endTimeStr);
        var infoString = JsonSerializer.Serialize(knownInfo);

        try
        {
            var message = await _anthropic.Messages.CreateAsync(new MessageRequest
            {
                Model = "claude-3-haiku-20240307",
                MaxTokens = 1000,
                Temperature = 0,
                Messages =
                [
                    new Message
                    {
                        Role = "user",
                        Content =
                            $"You are tasked with updating event information based on a given text input and existing known information. Both the input and output will be in JSON format.\n\nHere is the text input describing the event:\n<text>\n{prompt}\n</text>\n\nHere is the known information about the event in JSON format:\n<known_info>\n{infoString}\n</known_info>\n\nYour task is to update the event information based on the text input. Follow these steps:\n\n1. Analyze the text input for any information whether text input for the type is an outgoing or non-outgoing event.\n2. Analyze the text input for any information about the event name, destination, start time, or end time.\n3. analyze the text input to find information about the type of movement and output 1 if driving, 2 if train, 3 if walking, and 4 if another type.\n4. Compare this information with the known information provided in the JSON format.\n5. Update or add information to the JSON structure based on what you find in the text input.\n6. If any field cannot be determined from either the text input or the known information, set it to an empty string.\n\nProvide your output in the following JSON format:\n{{\n  \"IsOut\": boolean,\n  \"Remind\": \"Event name\",\n  \"To\": \"Destination\",\n  \"MoveType\" : int,\n  \"StartTime\": \"YYYY-MM-DDThh:mm\",\n  \"EndTime\": \"YYYY-MM-DDThh:mm\"\n}}\n\nMake sure to:\n- Overwrite existing information if new information is found in the text input.\n- Keep existing information if no new information is provided in the text input.\n- The endTime should be later than the startTime.\n- Use empty strings for any fields that cannot be determined from either source.\n\nHere's an example of how to process and output the information:\n\n<example>\nInput text: \"The company picnic will be held at Central Park by car on July 15th from 11 AM to 3 PM.\"\nKnown info: \n{{\n  \"IsOut\": false,\n  \"Remind\": \"Company Event\",\n  \"To\": \"\",\n  \"MoveType\": 0\n  \"StartTime\": \"\",\n  \"EndTime\": \"\"\n}}\n\nOutput:\n{{\n  \"IsOut\": true,\n  \"Remind\": \"Company Picnic\",\n  \"To\": \"Central Park\",\n  \"MoveType\": 1\n  \"StartTime\": \"2023-07-15T15:11\",\n  \"EndTime\": \"2023-07-15T15:15\"\n}}\n</example>\n\nNow, process the given text input and known information, and provide your output in the only specified JSON format."
                    }
                ]
            });

            logger.LogInformation("Anthropic API response: {msg}", message);
            var responseInfo = JsonSerializer.Deserialize<ClaudeFormat>(message.ToString());
            if (responseInfo == null)
                throw new RpcException(new Status(StatusCode.Internal, "Claude API error"));

            var startTime = GetDateTime(responseInfo.StartTime);
            var endTime = GetDateTime(responseInfo.EndTime);

            if (eventMaterial == null)
                return new EventMaterial
                {
                    IsOut = responseInfo.IsOut,
                    Remind = responseInfo.Remind,
                    Destination = responseInfo.To,
                    MoveType = (MoveType)responseInfo.MoveType,
                    StartTime = startTime,
                    EndTime = endTime
                };
            eventMaterial.IsOut = responseInfo.IsOut;
            eventMaterial.Remind = responseInfo.Remind;
            eventMaterial.Destination = responseInfo.To;
            eventMaterial.MoveType = (MoveType)responseInfo.MoveType;
            eventMaterial.StartTime = startTime;
            eventMaterial.EndTime = endTime;
            return eventMaterial;

        }
        catch (ClaudiaException ex)
        {
            throw new RpcException(new Status(StatusCode.Internal,
                $"Claude API error | {ex.Status} | {ex.Name} | {ex.Message}"));
        }
    }

    private static string GetDateTimeString(DateTime? dateTime)
    {
        if (dateTime == null)
            return "";
        return dateTime.Year + "-" + dateTime.Month + "-" + dateTime.Day + "T" + dateTime.Hour + ":" + dateTime.Minute;
    }

    private static DateTime GetDateTime(string dateTimeStr)
    {
        var dateTime = new DateTime();

        if (dateTimeStr == "")
            return dateTime;

        var dateTimeArray = dateTimeStr.Split('T');
        var dateArray = dateTimeArray[0].Split('-');
        var timeArray = dateTimeArray[1].Split(':');
        dateTime.Year = uint.Parse(dateArray[0]);
        dateTime.Month = uint.Parse(dateArray[1]);
        dateTime.Day = uint.Parse(dateArray[2]);
        dateTime.Hour = uint.Parse(timeArray[0]);
        dateTime.Minute = uint.Parse(timeArray[1]);
        return dateTime;
    }
}

public class ClaudeFormat(bool isOut, string remind, string to, int moveType, string startTime, string endTime)
{
    public ClaudeFormat(string remind, string to, string startTime, string endTime) : this(false, remind, to, 0,
        startTime, endTime)
    {
    }

    public bool IsOut { get; init; } = isOut;
    public string Remind { get; init; } = remind;
    public string To { get; init; } = to;
    public int MoveType { get; init; } = moveType;
    public string StartTime { get; init; } = startTime;
    public string EndTime { get; init; } = endTime;
}
