using GrpcDemo.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Это gRPC endpoint. Используйте gRPC-клиент.");

app.Run();
