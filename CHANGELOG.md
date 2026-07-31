# Changelog

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
- Pre-processors run before the pipeline; post-processors run after a successful pipeline (unchanged behaviour, now clearly documented).