![NuGet](https://img.shields.io/nuget/v/Interlink)
![.NET](https://img.shields.io/badge/.NET-8%20%7C%209-blueviolet)

# Interlink

**Interlink** is a lightweight and modern mediator library for .NET, designed to decouple your code through request/response and notification patterns. Built with simplicity and performance in mind, it helps streamline communication between components while maintaining a clean architecture.

---

## ✨ Features

- 🧩 Simple mediator pattern for request/response
- 🔁 Publish/Subscribe notification system
- 🔧 Pipeline behaviors (logging, validation, etc.)
- 🧠 Clean separation of concerns via handlers
- 🪝 Dependency injection support out of the box
- 🔄 Decouples logic using handlers
- 🧩 Easy registration with AddInterlink()
- 🚀 Lightweight, fast, and no external dependencies
- 🔄 Pre and Post Processors for enhanced lifecycle control
- ✅ Compatible with .NET 8 and .NET 9
- 🔍 Assembly scanning for automatic handler registration
- 🧪 Custom service factory injection
- 🔄 Pipeline ordering via attributes or configuration
- 🔄 Handler resolution caching (delegate-based)

---

## 💡 Why Interlink?
- Clean, intuitive API
- No bloat – just powerful mediation
- Perfect for CQRS, Clean Architecture, Modular Design
- Highly extensible with behaviors and notifications

---

## 📦 Installation
Install Interlink via NuGet:
```bash
dotnet add package Interlink
```

## ⚙️ Setup

Register Interlink in your `Startup.cs` or `Program.cs`
```csharp
builder.Services.AddInterlink();
```
You can optionally pass an Assembly:
```csharp
builder.Services.AddInterlink(typeof(MyHandler).Assembly);
```

## 📨 Request/Response Pattern
### 1. Define a request:
```csharp
public class GetAllPets
{
    public record Query : IRequest<List<string>>;

    public class Handler : IRequestHandler<Query, List<string>>
    {
        public Task<List<string>> Handle(Query request, CancellationToken cancellationToken)
        {
            var pets = new List<string> { "Dog", "Cat", "Fish" };
            return Task.FromResult(pets);
        }
    }
}
```

### 2. Inject and use ISender:
```csharp
[ApiController]
[Route("api/[controller]")]
public class PetController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllPets(CancellationToken cancellationToken)
    {
        var pets = await sender.Send(new GetAllPets.Query(), cancellationToken);
        return Ok(pets);
    }
}
```
---

## 📣 Notifications (One-to-Many)
### 1. Define a notification:
```csharp
public class UserCreated(string userName) : INotification
{
    public string UserName { get; } = userName;
}
```
### 2. Create one or more handlers:
```csharp
public class SendWelcomeEmail : INotificationHandler<UserCreated>
{
    public Task Handle(UserCreated notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Welcome email sent to {notification.UserName}");
        return Task.CompletedTask;
    }
}
```

### 3. Publish with IPublisher:
```csharp
public class AccountService(IPublisher publisher)
{
    public async Task RegisterUser(string username)
    {
        // Save to DB...
        await publisher.Publish(new UserCreated(username));
    }
}
```
---

## 🧬 Pipeline Behaviors
Useful for logging, validation, performance monitoring, etc.

### 1. Define a behavior:
```csharp
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        Console.WriteLine($"Handling {typeof(TRequest).Name}");
        var response = await next();
        Console.WriteLine($"Handled {typeof(TRequest).Name}");
        return response;
    }
}
```

Pipeline behaviors can be manually registered like this:

```csharp
builder.Services.AddInterlink(config =>
{
    config.AddBehavior(typeof(LoggingBehavior<,>));
    config.AddBehavior(typeof(LoggingBehavior2<,>)); // Add more behaviors as needed
});
```
---

Pipeline ordering can be controlled via attributes or configuration.
```csharp
[PipelineOrder(1)]
public class FirstBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        Console.WriteLine("First behavior executed");
        return await next();
    }
}

[PipelineOrder(2)]
public class SecondBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        Console.WriteLine("Second behavior executed");
        return await next();
    }
}
```
You can also use the `AddInterlink()` method to specify the order of pipeline behaviors:
```csharp
builder.Services.AddInterlink(config =>
{
    config.AddBehavior<FirstBehavior<GetAllPets.Query, List<string>>>(1);
    config.AddBehavior<SecondBehavior<GetAllPets.Query, List<string>>>(2);
});
```

## 🔄 Pre and Post Processor

### 1. Define Pre and Post Processors:

```csharp
public class MyRequestPreProcessor : IRequestPreProcessor<GetAllPets.Query>
{
    public Task Process(GetAllPets.Query request, CancellationToken cancellationToken)
    {
        Console.WriteLine("[PreProcessor] Processing GetAllPets request...");
        return Task.CompletedTask;
    }
}

public class MyRequestPostProcessor : IRequestPostProcessor<GetAllPets.Query, List<Pet>>
{
    public Task Process(GetAllPets.Query request, List<Pet> response, CancellationToken cancellationToken)
    {
        Console.WriteLine("[PostProcessor] Processing GetAllPets response...");
        return Task.CompletedTask;
    }
}
```
Pre and Post Processors are automatically registered when you call AddInterlink().

---

## 📦 API Overview

### ```IRequest<TResponse>```

```csharp
public interface IRequest<TResponse> { }
```

### ```IRequestHandler<TRequest, TResponse>```

```csharp
public interface IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
```

### ```INotification```

```csharp
public interface INotification { }
```

### ```INotificationHandler<TNotification>```

```csharp
public interface INotificationHandler<TNotification>
    where TNotification : INotification
{
    Task Handle(TNotification notification, CancellationToken cancellationToken);
}
```


### ```ISender```

```csharp
public interface ISender
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}
```

### ```IPublisher```

```csharp
public interface IPublisher
{
    Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification;
}
```

### ```IPipelineBehavior<TRequest, TResponse>```

```csharp
public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();

public interface IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next);
}
```

### ```IRequestPreProcessor<TRequest>```

```csharp
public interface IRequestPreProcessor<TRequest>
{
    Task Process(TRequest request, CancellationToken cancellationToken);
}
```

### ```IRequestPostProcessor<TRequest, TResponse>```

```csharp
public interface IRequestPostProcessor<TRequest, TResponse>
{
    Task Process(TRequest request, TResponse response, CancellationToken cancellationToken);
}
```

----

## 🔍 Example Use Case

- CQRS: Use ```IRequest<TResponse>``` for Queries and Commands
- Event-Driven architecture: Use ```INotification``` for broadcasting domain events
- Middleware-style behaviors: Add ```IPipelineBehavior``` for logging, validation, caching, etc.

----

## 🚀 Roadmap

### ✅ v1.0.0 — Core Mediator Basics (Released)
- Basic `IRequest<TResponse>` and `IRequestHandler<TRequest, TResponse>`
- `ISender` for sending requests
- `AddInterlink()` for automatic DI registration
- Clean, lightweight design
- Only .NET 9 support

### ✅ v1.0.1 — Core Mediator Basics (Released)
- Basic `IRequest<TResponse>` and `IRequestHandler<TRequest, TResponse>`
- `ISender` for sending requests
- `AddInterlink()` for automatic DI registration
- Clean, lightweight design
- .NET 8+ support

### ✅ v1.1.0 — Notifications & Pipelines (Released)
- `INotification` and `INotificationHandler<TNotification>`
- `IPublisher` for event broadcasting
- `IPipelineBehavior<TRequest, TResponse>` support
- Enhanced `AddInterlink()` with scanning and registration for notifications and pipelines
- Updated documentation and examples
- .NET 8+ support

### ✅ v1.2.0 — Pre/Post Processors (Released)
- `IRequestPreProcessor<TRequest>` interface
- `IRequestPostProcessor<TRequest, TResponse>` interface
- Pre and post hooks for request lifecycle
- Optional unit-of-work behaviors

### ✅ v1.2.1 — Fix Critical Bugs (Released)
- Fix critical bugs in `IPipelineBehavior<TRequest, TResponse>`

### ✅ v1.3.0 — Performance & Customization (Released)
- Handler resolution caching (delegate-based)
- Custom service factory injection support
- Pipeline ordering via attributes or configuration
- Assembly scanning filters by namespace or attribute

### 🌐 v1.4.0 — Extensions
- `Interlink.Extensions.Logging` — built-in logging behavior
- `Interlink.Extensions.Validation` — integration with FluentValidation
- `Interlink.AspNetCore` — model binding & filters for ASP.NET Core

### 🧪 v1.5.0 — Developer Experience
- Source generator / Roslyn analyzer for missing handler detection
- Code snippets and templates for common patterns
- Custom exception types (e.g., `HandlerNotFoundException`)

### 📅 Future Ideas
- Request cancellation and timeout behaviors
- Metrics collection and tracing support
- Dynamic or externalized pipeline config (e.g., JSON-based)

---

_Stay tuned for more updates! Contributions and suggestions are welcome._ ✨
---

## 📜 License
MIT License © ManuHub

---