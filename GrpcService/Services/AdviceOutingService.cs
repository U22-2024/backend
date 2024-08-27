using Advice.V1;
using Grpc.Core;
using GrpcService.API;

namespace GrpcService.Services;

public class AdviceOutingService(IConfiguration _config): Advice.V1.AdviceOutingService.AdviceOutingServiceBase {
    public override async Task<GetAdviceResponse> GetAdvice(GetAdviceRequest request, ServerCallContext context) {

        var getRainfall = new GetRainfall(_config);
        var rainfallList = await getRainfall.GetListRainfall(new API.Location {latitude =  request.Pos.Latitude, longitude = request.Pos.Longitude});

        double max = 0;

        foreach  (var rainfall in rainfallList) {
            max =  Math.Max(max, rainfall.Rainfall);
        }

        return new GetAdviceResponse{IsRequireUmbrella = max >= 0.5, MaxRainfall = max};
    }
}
