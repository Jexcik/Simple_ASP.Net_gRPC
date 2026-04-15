# Simple ASP.NET Core + gRPC demo

Ниже — минимальный, но показательный пример, чтобы быстро понять механику gRPC в ASP.NET Core.

## Что добавлено

В демо есть уже **3 типа взаимодействия**:

1. **Unary RPC** `SayHello` — один запрос, один ответ.
2. **Unary RPC** `GetServerTime` — возврат серверного времени.
3. **Server Streaming RPC** `SayHelloStream` — один запрос, поток ответов.

Это помогает сразу увидеть, как выглядит gRPC не только в самом простом кейсе, но и в стриминговом.

## Структура

- `src/GrpcDemo.Server` — ASP.NET Core gRPC сервер
- `src/GrpcDemo.Client` — консольный gRPC клиент
- `src/GrpcDemo.Server/Protos/greet.proto` — контракт, общий для клиента и сервера

## Контракт (`.proto`)

```proto
service Greeter {
  rpc SayHello (HelloRequest) returns (HelloReply);
  rpc GetServerTime (TimeRequest) returns (TimeReply);
  rpc SayHelloStream (HelloRequest) returns (stream HelloReply);
}
```

## Как это работает

1. Описываешь сервис в `greet.proto`.
2. `Grpc.Tools` генерирует C#-типы.
3. Сервер наследуется от `GreeterBase` и реализует методы.
4. Клиент создаёт `GreeterClient` и вызывает RPC как обычные async-методы.

## Запуск локально

> В этом контейнере `dotnet` не установлен, но на твоей машине команды будут такими:

```bash
dotnet restore

# терминал 1: сервер
dotnet run --project src/GrpcDemo.Server

# терминал 2: клиент
dotnet run --project src/GrpcDemo.Client
```

## Что посмотреть в клиенте

- вызов `SayHelloAsync`
- вызов `GetServerTimeAsync`
- чтение `await foreach` из `SayHelloStream(...).ResponseStream.ReadAllAsync()`

## Идеи для следующего шага

- добавить **client streaming** (клиент отправляет поток)
- добавить **bidirectional streaming**
- добавить обработку deadline/cancellation на клиенте
- вынести общие proto-контракты в отдельный проект
