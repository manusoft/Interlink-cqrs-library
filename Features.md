### High value (good next release — 1.6)

| Feature | Why |
|---------|-----|
| **`IRequest` (void / unit response)** | Commands that return nothing are awkward as `IRequest<Unit>`. Add `IRequest` + `IRequestHandler<TRequest>` (or a built-in `Unit` type). |
| **Streaming / `IAsyncEnumerable` responses** | `IStreamRequest<T>` + `IStreamRequestHandler` for large reads, exports, AI token streams. |
| **Notification publish strategies** | Today handlers run sequentially. Add `PublishStrategy`: sequential, parallel, stop-on-first-exception. |
| **Pipeline for notifications** | `INotificationPipelineBehavior<T>` for logging/validation around `Publish`. |
| **Better diagnostics** | Analyzer: multiple handlers for same request; handler registered but request type missing; optional severity config. |
| **Source generator registration** | Optional compile-time handler registration to reduce reflection/scanning at startup (AOT-friendly). |

### Medium value (1.7)

| Feature | Why |
|---------|-----|
| **Timeout / cancellation behaviors** | Built-in `TimeoutBehavior` via `IPipelineBehavior`. |
| **OpenTelemetry / metrics package** | `Interlink.Extensions.Telemetry` — activity per request, counters, histograms. |
| **Exception pipeline** | `IRequestExceptionHandler<TRequest, TException>` or centralized exception behaviors (MediatR-style). |
| **Keyed / constrained handlers** | Optional attribute or marker for “only run this behavior for commands” vs queries. |
| **`Send` overloads** | `Send(object request)` for dynamic scenarios; `CreateStream`. |
| **Minimal API helpers** | `MapPost<TRequest, TResponse>` style extensions in `Interlink.AspNetCore`. |

### Polish & ecosystem

| Item | Notes |
|------|--------|
| **AOT / trimming annotations** | `[DynamicallyAccessedMembers]` where needed; document AOT limits. |
| **Performance** | Cache open pipeline delegates per request type (you already cache scans; push further). |
| **Samples** | Vertical-slice sample, Minimal API sample, Blazor sample. |
| **Snippets / `dotnet new` template** | Request+handler, notification+handler, behavior. |
| **Docs site** | Docfx or simple GitHub Pages — README is good; a small site scales better. |

### Align with my old roadmap

From earlier docs, still open:

- Request cancellation / timeout behaviors  
- Metrics & tracing  
- Dynamic / JSON pipeline config (lower priority — often overkill)

### Suggested 1.6 scope (focused)

Keep the release small and shippable:

1. `IRequest` + `Unit` (or void handlers)  
2. Notification publish strategies (sequential / parallel)  
3. Analyzer improvements (duplicate handler, clearer messages)  
4. Optional `Interlink.Extensions.Telemetry` (basic Activity only)  

### What I would **not** prioritize yet

- Full event-sourcing toolkit  
- Saga / process manager framework  
- Replacing DI with a custom container  
- JSON-driven pipeline config (unless users ask)

---

