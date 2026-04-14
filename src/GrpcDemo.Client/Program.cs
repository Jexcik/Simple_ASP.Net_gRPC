using GrpcDemo.Server;
using Grpc.Net.Client;

using var channel = GrpcChannel.ForAddress("https://localhost:5001");
var client = new Greeter.GreeterClient(channel);

var reply = await client.SayHelloAsync(new HelloRequest
{
    Name = "ASP.NET Core learner"
});

Console.WriteLine($"Ответ сервера: {reply.Message}");
