![Static Badge](https://img.shields.io/badge/Interlink.AspNetCore-blue)
![NuGet Version](https://img.shields.io/nuget/v/Interlink.AspNetCore)
![NuGet Downloads](https://img.shields.io/nuget/dt/Interlink.AspNetCore)

# Interlink.AspNetCore

ASP.NET Core integration for the [Interlink](https://www.nuget.org/packages/Interlink) mediator library.

## Installation

```bash
dotnet add package Interlink.AspNetCore
```

Requires the core package:

```bash
dotnet add package Interlink
```

## Usage

```csharp
builder.Services.AddControllers();
builder.Services.AddInterlink(typeof(MyHandler).Assembly);
builder.Services.AddInterlinkAspNetCore();   // registers InterlinkExceptionFilter
```

## What it does

Adds a global `IExceptionFilter` that maps common Interlink exceptions to ProblemDetails responses:

| Exception                       | HTTP Status | Response type              |
|--------------------------------|-------------|----------------------------|
| `HandlerNotFoundException`     | 404         | `ProblemDetails`           |
| `FluentValidation.ValidationException`* | 400 | `ValidationProblemDetails` |

\* FluentValidation support is optional and detected at runtime — there is **no hard dependency** on the FluentValidation package.

## License

MIT © ManuHub