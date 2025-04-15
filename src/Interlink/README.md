# Interlink

**Interlink** is a lightweight and modern mediator library for .NET, designed to decouple your code through request/response and notification patterns. Built with simplicity and performance in mind, it helps streamline communication between components while maintaining a clean architecture.

---

## ✨ Features

- 🧩 Simple mediator pattern for request/response
- 🔄 Decouples logic using handlers
- 🧩 Easy registration with `AddInterlink()`
- 🔧 Supports dependency injection with `IServiceProvider`
- 🚀 Lightweight, fast, and no external dependencies
- ✅ Fully compatible with .NET 8, .NET 9

---

# 💡 Why Interlink?
- Clean, intuitive API
- No bloat, just mediation
- Ideal for CQRS, Clean Architecture, and modular designs

---

## 🛠️ Usage

### 1. Install Interlink
You can install Interlink via NuGet Package Manager or .NET CLI.
```bash
dotnet add package Interlink
```

### 1. Register Interlink in your `Startup.cs` or `Program.cs`
```csharp
using Interlink;

builder.Services.AddInterlink();

```

### 2. Define a Request and Response
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

# 📦 API Overview

```IRequest<TResponse>```

Marker interface for requests that return a TResponse.

```csharp
public interface IRequest<TResponse> { }
```

```IRequestHandler<TRequest, TResponse>```

Handles the request and returns a response.

```csharp
public interface IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
```

```ISender```

Used to send requests to the appropriate handler.

```csharp
public interface ISender
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}
```

----

# ⚙️ How it Works

```AddInterlink()``` scans the provided assembly for all classes that implement ```IRequestHandler<TRequest, TResponse>``` and registers them with the DI container. The ```Sender``` uses ```IServiceProvider``` to resolve and invoke the matching handler.

----

# 📜 License
MIT License © ManuHub