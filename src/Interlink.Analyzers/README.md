![Static Badge](https://img.shields.io/badge/Interlink.Analyzers-blue)
![NuGet Version](https://img.shields.io/nuget/v/Interlink.Analyzers)
![NuGet Downloads](https://img.shields.io/nuget/dt/Interlink.Analyzers)

# Interlink.Analyzers

Roslyn analyzer for the [Interlink](https://www.nuget.org/packages/Interlink) mediator library.

## Installation

```bash
dotnet add package Interlink.Analyzers
```

This is a development dependency (analyzer only). It does not add any runtime assemblies.

## What it detects

**ILINK001** – Missing request handler  

Raised when a type implements `IRequest<TResponse>` but no corresponding  
`IRequestHandler<TRequest, TResponse>` is found in the compilation.

### Example

```csharp
// Warning ILINK001: No handler found for request type 'GetAllPets.Query'
public sealed record GetAllPetsQuery : IRequest<List<string>>;
```

Add a handler to clear the diagnostic:

```csharp
public sealed class GetAllPetsHandler : IRequestHandler<GetAllPetsQuery, List<string>>
{
    public Task<List<string>> Handle(GetAllPetsQuery request, CancellationToken cancellationToken)
        => Task.FromResult(new List<string>());
}
```

## License

MIT © ManuHub