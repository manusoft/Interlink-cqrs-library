![Static Badge](https://img.shields.io/badge/Interlink.Extensions.Logging-blue)
![NuGet Version](https://img.shields.io/nuget/v/Interlink.Extensions.Logging)
![NuGet Downloads](https://img.shields.io/nuget/dt/Interlink.Extensions.Logging)

# Interlink.Extensions.Logging

Built-in logging pipeline behavior for the [Interlink](https://www.nuget.org/packages/Interlink) mediator library.

## Installation

```bash
dotnet add package Interlink.Extensions.Logging
```

Requires the core package:

```bash
dotnet add package Interlink
```

## Usage

```csharp
builder.Services.AddInterlink(typeof(MyHandler).Assembly);
builder.Services.AddInterlinkLogging();
```

This registers `LoggingBehavior<TRequest, TResponse>`, which logs:

- Request start
- Successful completion + elapsed milliseconds
- Exceptions (with stack trace)

### Example output

```
info: Handling GetAllPets.Query
info: Handled GetAllPets.Query successfully in 12 ms
```

## Customization

The behavior uses `ILogger<LoggingBehavior<TRequest, TResponse>>`.  
Configure logging levels and providers as usual with Microsoft.Extensions.Logging.

## License

MIT © ManuHub