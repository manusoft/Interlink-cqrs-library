# Changelog

## [1.5.2] - 2026-08-13

### Added
- **Unified `IMediator`** – new interface that inherits from both `ISender` and `IPublisher`.
- **`Mediator` implementation** – lightweight composition of the existing `Sender` and `Publisher`.
- `IMediator` is now registered by default in `AddInterlink()`.

### Changed
- Users can now inject `IMediator`, `ISender`, or `IPublisher` independently (full backward compatibility).

---

## [1.5.1] - 2026-08-XX

### Added
- **`Unit` support** – non-generic `IRequest` (equivalent to `IRequest<Unit>`) for fire-and-forget / void commands.
- `Unit` struct with `Unit.Value`.
- Convenience overload `ISender.Send(IRequest request)` that maps to `Send<Unit>()`.

### Changed
- Documentation and examples updated to show `Unit` usage.

---

## [1.5.0] - 2026-07-31

### Added
- **HandlerNotFoundException** – dedicated exception type thrown when a request has no registered handler.
- **Pipeline ordering** – `PipelineOrderAttribute` is now respected; behaviors are sorted ascending (lowest order runs outermost).
- **Custom service factory** – fully wired via `InterlinkOptions.ServiceFactory`.
- **Interlink.Extensions.Logging** – `LoggingBehavior<TRequest,TResponse>` + `AddInterlinkLogging()`.
- **Interlink.Extensions.Validation** – FluentValidation integration (`ValidationBehavior` + `AddInterlinkValidation()` with optional assembly scanning).
- **Interlink.AspNetCore** – `InterlinkExceptionFilter` (maps `HandlerNotFoundException` and optional FluentValidation exceptions to ProblemDetails) + `AddInterlinkAspNetCore()`.
- **Interlink.Analyzers** – Roslyn analyzer (`ILINK001`) that warns when an `IRequest<T>` has no corresponding `IRequestHandler`.
- XML documentation comments on all public APIs.

### Fixed
- Pipeline behavior signature documentation now matches the actual `Handle(request, next, cancellationToken)` order.
- Removed dead / commented-out code in `ServiceCollectionExtensions`.
- Removed unused `HandlerResolver`.
- Consolidated type-scanning cache.
- Publisher now uses the same dynamic dispatch style as Sender (no reflection `Invoke`).
- `InterlinkOptions.AddBehavior` supports an optional explicit order parameter.
- Proper null checks and `ArgumentNullException` on public entry points.

### Changed
- Version bumped to 1.5.0.
- `Sender` and `Publisher` are now `sealed`.
- Pre-processors run before the pipeline; post-processors run after a successful pipeline.

---

## [1.4.0] - Previous

### Changed
- Officially targets .NET Standard 2.0+ (improved multi-targeting and compatibility)

### Fixed
- Minor packaging and target framework alignment issues

---

## [1.3.1] - Previous

### Changed
- Support .NET Standard 2.0+ (works in .NET Core, .NET 5+, .NET Framework 4.7.2+)

---

## [1.3.0] — Performance & Customization

### Added
- Handler resolution caching (delegate-based)
- Custom service factory injection support
- Pipeline ordering via attributes or configuration
- Assembly scanning filters by namespace or attribute

---

## [1.2.1] — Fix Critical Bugs

### Fixed
- Critical bugs in `IPipelineBehavior<TRequest, TResponse>`

---

## [1.2.0] — Pre/Post Processors

### Added
- `IRequestPreProcessor<TRequest>` interface
- `IRequestPostProcessor<TRequest, TResponse>` interface
- Pre and post hooks for request lifecycle
- Optional unit-of-work behaviors

---

## [1.1.0] — Notifications & Pipelines

### Added
- `INotification` and `INotificationHandler<TNotification>`
- `IPublisher` for event broadcasting
- `IPipelineBehavior<TRequest, TResponse>` support
- Enhanced `AddInterlink()` with scanning and registration for notifications and pipelines
- Updated documentation and examples
- .NET 8+ support

---

## [1.0.1] — Core Mediator Basics

### Changed
- .NET 8+ support

### Added
- Basic `IRequest<TResponse>` and `IRequestHandler<TRequest, TResponse>`
- `ISender` for sending requests
- `AddInterlink()` for automatic DI registration
- Clean, lightweight design

---

## [1.0.0] — Core Mediator Basics

### Added
- Basic `IRequest<TResponse>` and `IRequestHandler<TRequest, TResponse>`
- `ISender` for sending requests
- `AddInterlink()` for automatic DI registration
- Clean, lightweight design
- Only .NET 9 support