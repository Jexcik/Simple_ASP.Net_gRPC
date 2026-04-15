using GrpcDemo.Server;
using Grpc.Net.Client;

using var channel = GrpcChannel.ForAddress("https://localhost:5001");
var client = new Greeter.GreeterClient(channel);

Console.WriteLine("=== Unary: SayHello ===");
var helloReply = await client.SayHelloAsync(new HelloRequest
{
    Name = "ASP.NET Core learner"
});
Console.WriteLine($"Ответ: {helloReply.Message}");

Console.WriteLine();
Console.WriteLine("=== Unary: GetServerTime ===");
var timeReply = await client.GetServerTimeAsync(new TimeRequest
{
    Timezone = "UTC"
});
Console.WriteLine($"UTC time: {timeReply.UtcTime}");
Console.WriteLine($"Timezone: {timeReply.Timezone}");

Console.WriteLine();
Console.WriteLine("=== Server Streaming: SayHelloStream ===");
using var streamCall = client.SayHelloStream(new HelloRequest
{
    Name = "Stream learner"
});

await foreach (var item in streamCall.ResponseStream.ReadAllAsync())
{
    Console.WriteLine($"Stream item: {item.Message}");
}
