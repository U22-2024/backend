using Grpc.Core;
using GrpcService.AI;
using GrpcService.Extensions;
using GrpcService.Models.Shopping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Shopping.V1;

namespace GrpcService.Services;

public class ShoppingService(AppDbContext dbContext, PredictShoppingItemCategory categoryPredictor)
    : Shopping.V1.ShoppingService.ShoppingServiceBase
{
    [Authorize]
    public override async Task<GetShoppingListResponse> GetShoppingList(GetShoppingListRequest request,
        ServerCallContext context)
    {
        var user = context.GetAuthUser();
        var res = new GetShoppingListResponse();
        res.Items.AddRange(await dbContext.ShoppingItems
            .Where(item => item.Uid == user.Uid)
            .Select(item => item.ToShoppingItem())
            .ToListAsync());

        return res;
    }

    [Authorize]
    public override async Task<CreateShoppingItemResponse> CreateShoppingItem(CreateShoppingItemRequest request,
        ServerCallContext context)
    {
        var user = context.GetAuthUser();

        if (string.IsNullOrWhiteSpace(request.Name))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Name is required"));
        var category = await categoryPredictor.PredictCategory(request.Name);
        var item = new ShoppingItemModel
        {
            Name = request.Name,
            Uid = user.Uid,
            Memo = request.Memo,
            Quantity = request.Quantity,
            Status = ItemStatus.NotPurchased,
            Type = category.ToString()
        };

        dbContext.ShoppingItems.Add(item);
        await dbContext.SaveChangesAsync();

        return new CreateShoppingItemResponse
        {
            Item = item.ToShoppingItem()
        };
    }

    [Authorize]
    public override async Task<UpdateShoppingItemResponse> UpdateShoppingItem(UpdateShoppingItemRequest request,
        ServerCallContext context)
    {
        var user = context.GetAuthUser();

        if (!Guid.TryParse(request.Id.Value, out var reqGuid))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid ID"));
        var item = await dbContext.ShoppingItems
            .FirstOrDefaultAsync(item => item.Uid == user.Uid && item.Id == reqGuid);
        if (item == null) throw new RpcException(new Status(StatusCode.NotFound, "Item not found"));

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var category = await categoryPredictor.PredictCategory(request.Name);
            item.Type = category.ToString();
        }
        item.Name = request.Name;
        item.Memo = request.Memo;
        item.Quantity = request.Quantity;
        item.Status = request.Status;
        await dbContext.SaveChangesAsync();

        return new UpdateShoppingItemResponse
        {
            Item = item.ToShoppingItem()
        };
    }

    [Authorize]
    public override async Task<DeleteShoppingItemResponse> DeleteShoppingItem(DeleteShoppingItemRequest request,
        ServerCallContext context)
    {
        var user = context.GetAuthUser();

        if (!Guid.TryParse(request.Id.Value, out var reqGuid))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid ID"));
        var item = await dbContext.ShoppingItems
            .FirstOrDefaultAsync(item => item.Uid == user.Uid && item.Id == reqGuid);
        if (item == null) throw new RpcException(new Status(StatusCode.NotFound, "Item not found"));

        dbContext.ShoppingItems.Remove(item);
        await dbContext.SaveChangesAsync();

        return new DeleteShoppingItemResponse();
    }
}
