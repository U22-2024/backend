using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// Postgresql を追加
var postgres = builder.AddPostgres("postgres");

// Grpcサーバーに外部HTTPエンドポイントとPostgresqlを追加
builder.AddProject<GrpcService>("grpc")
    .WithExternalHttpEndpoints()
    .WithReference(postgres);

// デプロイ用のマニフェストを作る時には実行しないように RunMode のときだけ DB 初期化のワーカーを追加
if (builder.ExecutionContext.IsRunMode)
    // DB 初期化のワーカー
    builder.AddProject<DbInitializer>("db-initializer")
        .WithReference(postgres);

builder.Build().Run();