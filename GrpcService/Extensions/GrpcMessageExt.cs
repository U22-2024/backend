using GrpcService.Models.Shopping;
using Shopping.V1;

namespace GrpcService.Extensions;

public static class GrpcMessageExt
{
    public static ShoppingItem ToShoppingItem(this ShoppingItemModel self)
    {
        return new ShoppingItem
        {
            Id = self.Id.ToGrpcGuid(),
            Name = self.Name,
            Quantity = self.Quantity,
            Type = self.Type,
            Memo = self.Memo,
            Status = self.Status
        };
    }

    public static ShoppingItemModel ToShoppingItemModel(this ShoppingItem self, string uid)
    {
        return new ShoppingItemModel
        {
            Id = Guid.Parse(self.Id.Value),
            Name = self.Name,
            Quantity = self.Quantity,
            Type = self.Type,
            Memo = self.Memo,
            Status = self.Status,
            Uid = uid
        };
    }
}
