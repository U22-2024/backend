using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// Postgresql を追加
var postgres = builder
    .AddPostgres("postgres");

// Grpcサーバーに外部HTTPエンドポイントとPostgresqlを追加
builder.AddProject<GrpcService>("grpc")
    .WithExternalHttpEndpoints()
    .WithReference(postgres);

builder.Build().Run();
