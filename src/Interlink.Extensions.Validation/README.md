![Static Badge](https://img.shields.io/badge/Interlink.Extensions.Validation-blue)
![NuGet Version](https://img.shields.io/nuget/v/Interlink.Extensions.Validation)
![NuGet Downloads](https://img.shields.io/nuget/dt/Interlink.Extensions.Validation)

# Interlink.Extensions.Validation

FluentValidation integration for the [Interlink](https://www.nuget.org/packages/Interlink) mediator library.

## Installation

```bash
dotnet add package Interlink.Extensions.Validation
```

Requires:

```bash
dotnet add package Interlink
dotnet add package FluentValidation
```

## Usage

Register the validation behavior (and optionally scan for validators):

```csharp
// Registers ValidationBehavior only
builder.Services.AddInterlinkValidation();

// Registers ValidationBehavior + scans assemblies for IValidator<T>
builder.Services.AddInterlinkValidation(typeof(CreateUserValidator).Assembly);
```

### Example validator

```csharp
public sealed class CreateUserValidator : AbstractValidator<CreateUser.Command>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}
```

When validation fails, a `FluentValidation.ValidationException` is thrown before the handler runs.

## How it works

`ValidationBehavior<TRequest, TResponse>` resolves all `IValidator<TRequest>` instances from DI, runs them, and throws if any failures are found.

## License

MIT © ManuHub