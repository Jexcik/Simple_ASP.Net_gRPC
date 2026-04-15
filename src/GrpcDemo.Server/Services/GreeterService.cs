using Grpc.Core;

namespace GrpcDemo.Server.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;

    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Получен unary-запрос от {Name}", request.Name);

        return Task.FromResult(new HelloReply
        {
            Message = $"Привет, {request.Name}! Ответ от gRPC-сервера в {DateTime.UtcNow:O}"
        });
    }

    public override Task<TimeReply> GetServerTime(TimeRequest request, ServerCallContext context)
    {
        var timezone = string.IsNullOrWhiteSpace(request.Timezone) ? "UTC" : request.Timezone;

        _logger.LogInformation("Запрошено серверное время в зоне {Timezone}", timezone);

        return Task.FromResult(new TimeReply
        {
            UtcTime = DateTime.UtcNow.ToString("O"),
            Timezone = timezone
        });
    }

    public override async Task SayHelloStream(
        HelloRequest request,
        IServerStreamWriter<HelloReply> responseStream,
        ServerCallContext context)
    {
        _logger.LogInformation("Начинаем stream-ответ для {Name}", request.Name);

        for (var i = 1; i <= 3; i++)
        {
            if (context.CancellationToken.IsCancellationRequested)
            {
                _logger.LogWarning("Стрим был отменен клиентом");
                break;
            }

            await responseStream.WriteAsync(new HelloReply
            {
                Message = $"[{i}/3] Привет, {request.Name}!"
            });

            await Task.Delay(700, context.CancellationToken);
        }
    }
}
