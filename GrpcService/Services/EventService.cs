using Event.V1;
using Grpc.Core;
using GrpcService.Extensions;
using GrpcService.Models.Event;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DateTime = System.DateTime;

namespace GrpcService.Services;

public class EventService(AppDbContext dbContext) : Event.V1.EventService.EventServiceBase
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
            EventItems = request.EventItem?.ToArray() ?? [],
            UserItems = request.UserItems?.Item?.ToArray() ?? [],
            TransitCount = (int)(request.TimeTable?.TransitCount ?? 0),
            WalkDistance = (int)(request.TimeTable?.WalkDistance ?? 0),
            Fare = (int)(request.TimeTable?.Fare ?? 0),
            Uid = request.Uid.Value,
            TimeTableItems = new List<TimeTableItemModel>()
        };


        foreach (var timeTableItem in request.TimeTable?.Item ?? [])
        {
            var timeTableItemModel = ToTimeTableItemModel(timeTableItem, eventModel);
            eventModel.TimeTableItems.Add(timeTableItemModel);
        }

        var e = dbContext.Events.Add(eventModel);
        await dbContext.SaveChangesAsync();

        return new CreateEventResponse
        {
            Event = EventModel2GrpcEvent(e.Entity)
        };
    }

    [Authorize]
    public override async Task<GetEventResponse> GetEvent(GetEventRequest request, ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        var guid = Guid.Parse(request.Id.Value);
        var eventModel = await dbContext.Events
            .Include(e => e.TimeTableItems)
            .FirstAsync(e => e.Id == guid);
        if (eventModel == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Event not found"));
        if (authUser.Uid != request.Uid.Value)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied"));

        return new GetEventResponse
        {
            Event = EventModel2GrpcEvent(eventModel)
        };
    }

    [Authorize]
    public override async Task<GetEventsResponse> GetEvents(GetEventsRequest request, ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        if (authUser.Uid != request.Uid.Value)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied"));
        var eventModels = await dbContext.Events
            .Include(e => e.TimeTableItems)
            .Where(e => e.Uid == request.Uid.Value)
            .ToListAsync();
        var events = eventModels
            .Select(EventModel2GrpcEvent)
            .ToList();

        return new GetEventsResponse
        {
            Events = { events }
        };
    }

    [Authorize]
    public override async Task<UpdateEventResponse> UpdateEvent(UpdateEventRequest request, ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        var guid = Guid.Parse(request.Event.Id.Value);
        var eventModel = await dbContext.Events
            .Include(e => e.TimeTableItems)
            .FirstAsync(e => e.Id == guid);
        if (eventModel == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Event not found"));
        if (authUser.Uid != request.Uid.Value)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied"));

        eventModel.Title = request.Event.Title;
        eventModel.Description = request.Event.Description;
        eventModel.EventItems = request.Event?.EventItem?.ToArray() ?? [];
        eventModel.UserItems = request.Event?.UserItems?.Item.ToArray() ?? [];
        eventModel.TransitCount = (int)(request.Event?.TimeTable?.TransitCount ?? 0);
        eventModel.WalkDistance = (int)(request.Event?.TimeTable?.WalkDistance ?? 0);
        eventModel.Fare = (int)(request.Event?.TimeTable?.Fare ?? 0);
        eventModel.TimeTableItems.Clear();
        foreach (var timeTableItem in request.Event?.TimeTable?.Item ?? [])
        {
            var timeTableItemModel = ToTimeTableItemModel(timeTableItem, eventModel);
            dbContext.TimeTableItems.Add(timeTableItemModel);
        }

        var e = dbContext.Events.Update(eventModel);
        await dbContext.SaveChangesAsync();

        return new UpdateEventResponse
        {
            Event = EventModel2GrpcEvent(e.Entity)
        };
    }

    [Authorize]
    public override async Task<DeleteEventResponse> DeleteEvent(DeleteEventRequest request, ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        var guid = Guid.Parse(request.Id.Value);
        var eventModel = await dbContext.Events
            .FirstAsync(e => e.Id == guid);
        if (eventModel == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Event not found"));
        if (authUser.Uid != request.Uid.Value)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied"));

        dbContext.Events.Remove(eventModel);
        await dbContext.SaveChangesAsync();
        return new DeleteEventResponse();
    }

    public DateTime ToDateTime(Event.V1.DateTime dateTime)
    {
        return new DateTime((int)dateTime.Year, (int)dateTime.Month, (int)dateTime.Day, (int)dateTime.Hour,
            (int)dateTime.Minute, 0, DateTimeKind.Utc);
    }

    private static Event.V1.Event EventModel2GrpcEvent(EventModel eventModel)
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

    private TimeTableItemModel ToTimeTableItemModel(TimeTableItem timeTableItem, EventModel eventModel)
    {
        if (timeTableItem.Type == TimeTableType.Point)
            return new TimeTableItemModel
            {
                Type = (int)timeTableItem.Type,
                Name = timeTableItem.Name,
                Event = eventModel
            };

        if (timeTableItem.Move == "train")
            return new TimeTableItemModel
            {
                Event = eventModel,
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
            Event = eventModel,
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
