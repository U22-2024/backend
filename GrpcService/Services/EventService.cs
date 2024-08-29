using Event.V1;
using Grpc.Core;
using GrpcService.Extensions;
using GrpcService.Models.Event;
using Microsoft.AspNetCore.Authorization;
using DateTime = System.DateTime;

namespace GrpcService.Services;

public class EventService(AppDbContext dbContext, ILogger<EventService> logger) : Event.V1.EventService.EventServiceBase
{
    [Authorize]
    public override async Task<CreateEventResponse> CreateEvent(CreateEventRequest request, ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        if (authUser.Uid != request.Uid.Value)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied"));

        var eventModel = new EventModel
        {
            Title = request.Title,
            Description = request.Description,
            EventItems = request.EventItem.ToArray(),
            UserItems = request.UserItems.Item.ToArray(),

            TransitCount = (int)request.TimeTable.TransitCount,
            WalkDistance = (int)request.TimeTable.WalkDistance,
            Fare = (int)request.TimeTable.Fare,
            Uid = request.Uid.Value
        };

        var e = dbContext.Events.Add(eventModel);

        foreach (var timeTableItem in request.TimeTable.Item)
        {
            var timeTableItemModel = GetTimeTableItemModel(timeTableItem, e.Entity.Id);

            dbContext.TimeTableItems.Add(timeTableItemModel);
        }

        await dbContext.SaveChangesAsync();

        return new CreateEventResponse
        {
            Event = GetEvent(eventModel)
        };
    }

    [Authorize]
    public override async Task<GetEventResponse> GetEvent(GetEventRequest request, ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        var eventModel = await dbContext.Events.FindAsync(Guid.Parse(request.Id.Value));
        if (eventModel == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Event not found"));
        if (authUser.Uid != request.Uid.Value)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied"));

        return new GetEventResponse
        {
            Event = GetEvent(eventModel)
        };
    }

    [Authorize]
    public override Task<GetEventsResponse> GetEvents(GetEventsRequest request, ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        if (authUser.Uid != request.Uid.Value)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied"));
        var eventModels = dbContext.Events.Where(e => e.Uid == request.Uid.Value).ToList();
        var events = eventModels.Select(GetEvent).ToList();

        return Task.FromResult(new GetEventsResponse
        {
            Events = { events }
        });
    }

    [Authorize]
    public override async Task<UpdateEventResponse> UpdateEvent(UpdateEventRequest request, ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        var eventModel = await dbContext.Events.FindAsync(Guid.Parse(request.Event.Id.Value));
        if (eventModel == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Event not found"));
        if (authUser.Uid != request.Uid.Value)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied"));

        eventModel.Title = request.Event.Title;
        eventModel.Description = request.Event.Description;
        eventModel.EventItems = request.Event.EventItem.ToArray();
        eventModel.UserItems = request.Event.UserItems.Item.ToArray();
        eventModel.TransitCount = (int)request.Event.TimeTable.TransitCount;
        eventModel.WalkDistance = (int)request.Event.TimeTable.WalkDistance;
        eventModel.Fare = (int)request.Event.TimeTable.Fare;

        foreach (var timeTableItem in eventModel.TimeTableItems)
        {
            dbContext.TimeTableItems.Remove(timeTableItem);
            await dbContext.SaveChangesAsync();
        }

        var e = dbContext.Events.Update(eventModel);

        foreach (var timeTableItem in request.Event.TimeTable.Item)
        {
            var timeTableItemModel = GetTimeTableItemModel(timeTableItem, e.Entity.Id);

            dbContext.TimeTableItems.Add(timeTableItemModel);
        }

        await dbContext.SaveChangesAsync();

        return new UpdateEventResponse
        {
            Event = GetEvent(eventModel)
        };
    }

    [Authorize]
    public override async Task<DeleteEventResponse> DeleteEvent(DeleteEventRequest request, ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        var eventModel = await dbContext.Events.FindAsync(Guid.Parse(request.Id.Value));
        if (eventModel == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Event not found"));
        if (authUser.Uid != request.Uid.Value)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied"));

        foreach (var timeTableItem in eventModel.TimeTableItems) dbContext.TimeTableItems.Remove(timeTableItem);

        dbContext.Events.Remove(eventModel);
        await dbContext.SaveChangesAsync();

        return new DeleteEventResponse();
    }

    public DateTime ToDateTime(Event.V1.DateTime dateTime)
    {
        return new DateTime((int)dateTime.Year, (int)dateTime.Month, (int)dateTime.Day, (int)dateTime.Hour,
            (int)dateTime.Minute, 0, DateTimeKind.Utc);
    }

    public Event.V1.Event GetEvent(EventModel eventModel)
    {
        var userItems = new UserItems();
        userItems.Item.AddRange(eventModel.UserItems);

        return new Event.V1.Event
        {
            Id = new Common.V1.Guid { Value = eventModel.Id.ToString() },
            Title = eventModel.Title,
            Description = eventModel.Description,
            EventItem = { eventModel.EventItems },
            UserItems = userItems,
            TimeTable = new TimeTable
            {
                TransitCount = (uint)eventModel.TransitCount,
                WalkDistance = (uint)eventModel.WalkDistance,
                Fare = (uint)eventModel.Fare,
                Item =
                {
                    eventModel.TimeTableItems.Select(timeTableItem => timeTableItem.ToTimeTableItem())
                }
            }
        };
    }

    public TimeTableItemModel GetTimeTableItemModel(TimeTableItem timeTableItem, Guid eventId)
    {
        if (timeTableItem.Type == TimeTableType.Point)
            return new TimeTableItemModel
            {
                Type = (int)timeTableItem.Type,
                Name = timeTableItem.Name,
                EventId = eventId
            };

        if (timeTableItem.Move == "train")
            return new TimeTableItemModel
            {
                EventId = eventId,
                Type = (int)timeTableItem.Type,
                Name = timeTableItem.Name,
                Move = timeTableItem.Move,
                FromTime = ToDateTime(timeTableItem.FromTime),
                EndTime = ToDateTime(timeTableItem.EndTime),
                Distance = (int)timeTableItem.Distance,
                LineName = timeTableItem.LineName,
                Fare = (int)timeTableItem.Transport.Fare,
                TrainName = timeTableItem.Transport.TrainName,
                Color = timeTableItem.Transport.Color,
                Direction = timeTableItem.Transport.Direction,
                Destination = timeTableItem.Transport.Destination
            };

        return new TimeTableItemModel
        {
            EventId = eventId,
            Type = (int)timeTableItem.Type,
            Name = timeTableItem.Name,
            Move = timeTableItem.Move,
            FromTime = ToDateTime(timeTableItem.FromTime),
            EndTime = ToDateTime(timeTableItem.EndTime),
            Distance = (int)timeTableItem.Distance,
            LineName = timeTableItem.LineName
        };
    }
}
