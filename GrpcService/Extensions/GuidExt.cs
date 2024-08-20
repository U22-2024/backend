using Guid = Common.V1.Guid;

namespace GrpcService.Extensions;

public static class GuidExt
{
    public static Guid ToGrpcGuid(this System.Guid self)
    {
        return new Guid
        {
            Value = self.ToString()
        };
    }
}
