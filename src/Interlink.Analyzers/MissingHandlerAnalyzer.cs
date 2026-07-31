using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Interlink.Analyzers;

/// <summary>
/// Analyzer that reports a diagnostic when a type implementing <c>IRequest&lt;TResponse&gt;</c>
/// has no corresponding <c>IRequestHandler&lt;TRequest, TResponse&gt;</c> in the compilation.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class MissingHandlerAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// The diagnostic identifier for a missing request handler.
    /// </summary>
    public const string DiagnosticId = "ILINK001";

    private static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticId,
        title: "Missing request handler",
        messageFormat: "No handler found for request type '{0}'. Implement IRequestHandler<{0}, TResponse>.",
        category: "Interlink",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Every type that implements IRequest<TResponse> should have a corresponding IRequestHandler implementation registered or defined in the solution.");

    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(Rule);

    /// <inheritdoc />
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterCompilationStartAction(OnCompilationStart);
    }

    private static void OnCompilationStart(CompilationStartAnalysisContext context)
    {
        var requestInterface = context.Compilation.GetTypeByMetadataName("Interlink.Contracts.IRequest`1");
        var handlerInterface = context.Compilation.GetTypeByMetadataName("Interlink.IRequestHandler`2");

        if (requestInterface is null || handlerInterface is null)
            return; // Interlink not referenced

        var requestTypes = new List<INamedTypeSymbol>();
        var handlerRequestTypes = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);

        context.RegisterSymbolAction(symbolContext =>
        {
            if (symbolContext.Symbol is not INamedTypeSymbol namedType ||
                namedType.TypeKind != TypeKind.Class && namedType.TypeKind != TypeKind.Struct)
                return;

            foreach (var iface in namedType.AllInterfaces)
            {
                if (iface.OriginalDefinition.Equals(requestInterface, SymbolEqualityComparer.Default) &&
                    iface.TypeArguments.Length == 1)
                {
                    requestTypes.Add(namedType);
                }

                if (iface.OriginalDefinition.Equals(handlerInterface, SymbolEqualityComparer.Default) &&
                    iface.TypeArguments.Length == 2)
                {
                    var reqArg = iface.TypeArguments[0] as INamedTypeSymbol;
                    if (reqArg is not null)
                        handlerRequestTypes.Add(reqArg);
                }
            }
        }, SymbolKind.NamedType);

        context.RegisterCompilationEndAction(endContext =>
        {
            foreach (var requestType in requestTypes)
            {
                if (!handlerRequestTypes.Contains(requestType))
                {
                    var diagnostic = Diagnostic.Create(
                        Rule,
                        requestType.Locations.FirstOrDefault() ?? Location.None,
                        requestType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));

                    endContext.ReportDiagnostic(diagnostic);
                }
            }
        });
    }
}