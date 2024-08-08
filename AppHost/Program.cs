using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// Firebase Auth Emulatorを開発時のみ追加
#if DEBUG
builder.AddDockerfile("firebase", "Docker", "authEmulator.Dockerfile")
    .WithBindMount("./Docker/firebase/.firebaserc", "/opt/firebase/.firebaserc")
    .WithBindMount("./Docker/firebase/firebase.json", "/opt/firebase/firebase.json")
    .WithBindMount("./Docker/firebase/data", "/opt/firebase/data")
    .WithExternalHttpEndpoints()
    .WithHttpEndpoint(port: 9099, targetPort: 9099, name: "auth-emulator")
    .WithHttpEndpoint(port: 4000, targetPort: 4000, name: "auth-ui");
#endif

// Postgresql を追加
var postgres = builder.AddPostgres("postgres");

// Grpcサーバーに外部HTTPエンドポイントとPostgresqlを追加
builder.AddProject<GrpcService>("grpc")
    .WithExternalHttpEndpoints()
    .WithReference(postgres);

builder.Build().Run();
