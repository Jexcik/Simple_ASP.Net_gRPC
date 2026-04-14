# Simple ASP.NET Core + gRPC demo

Ниже — минимальный, но показательный пример, чтобы быстро понять механику gRPC в ASP.NET Core:

- `proto`-контракт (`greet.proto`) описывает сервис и сообщения.
- Сервер (`GrpcDemo.Server`) реализует RPC-метод `SayHello`.
- Клиент (`GrpcDemo.Client`) вызывает этот метод как обычный C#-метод.

## 1) Как это работает в 30 секунд

1. Ты описываешь API в `.proto` файле.
2. `Grpc.Tools` генерирует C# классы (клиент/серверные base-классы + DTO).
3. Сервер наследуется от `Greeter.GreeterBase` и реализует `SayHello`.
4. Клиент создает `Greeter.GreeterClient` и вызывает `SayHelloAsync`.

## 2) Контракт (proto)

Файл: `src/GrpcDemo.Server/Protos/greet.proto`

```proto
syntax = "proto3";

option csharp_namespace = "GrpcDemo.Server";

package greet;

service Greeter {
  rpc SayHello (HelloRequest) returns (HelloReply);
}

message HelloRequest {
  string name = 1;
}

message HelloReply {
  string message = 1;
}
```

## 3) Сервер

### Регистрация gRPC

Файл: `src/GrpcDemo.Server/Program.cs`

```csharp
builder.Services.AddGrpc();
app.MapGrpcService<GreeterService>();
```

### Реализация метода

Файл: `src/GrpcDemo.Server/Services/GreeterService.cs`

```csharp
public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
{
    return Task.FromResult(new HelloReply
    {
        Message = $"Привет, {request.Name}!"
    });
}
```

## 4) Клиент

Файл: `src/GrpcDemo.Client/Program.cs`

```csharp
using var channel = GrpcChannel.ForAddress("https://localhost:5001");
var client = new Greeter.GreeterClient(channel);

var reply = await client.SayHelloAsync(new HelloRequest
{
    Name = "ASP.NET Core learner"
});

Console.WriteLine(reply.Message);
```

## 5) Как запустить локально

> В этом контейнере `dotnet` не установлен, но на твоей машине команды будут такими:

```bash
# из корня репозитория
dotnet restore

# терминал 1: сервер
dotnet run --project src/GrpcDemo.Server

# терминал 2: клиент
dotnet run --project src/GrpcDemo.Client
```

## 6) Что важно понять новичку

- **gRPC = контракт-first**: сначала `.proto`, потом реализация.
- **HTTP/2 + protobuf**: быстрее и компактнее JSON в типичных внутренних API.
- **Сильная типизация**: меньше ошибок на клиенте.
- **Отлично для микросервисов** внутри .NET экосистемы.

## 7) Следующий шаг для практики

Добавь второй RPC метод, например:
- `GetServerTime(Empty) returns (ServerTimeReply)`

и вызови его из клиента. Так ты закрепишь:
- изменение контракта,
- автогенерацию кода,
- расширение сервиса и клиента.
