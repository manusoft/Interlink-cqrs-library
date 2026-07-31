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

    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        id: DiagnosticId,
        title: "Missing request handler",
        messageFormat: "No handler found for request type '{0}'. Implement IRequestHandler<{0}, TResponse>.",
        category: "Interlink",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Every type that implements IRequest<TResponse> should have a corresponding IRequestHandler implementation defined in the compilation.",
        helpLinkUri: null,
        customTags: new[] { WellKnownDiagnosticTags.CompilationEnd });

    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(Rule);

    /// <inheritdoc />
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterCompilationAction(AnalyzeCompilation);
    }

    private static void AnalyzeCompilation(CompilationAnalysisContext context)
    {
        var compilation = context.Compilation;

        var requestInterface = compilation.GetTypeByMetadataName("Interlink.Contracts.IRequest`1");
        var handlerInterface = compilation.GetTypeByMetadataName("Interlink.IRequestHandler`2");

        // Interlink not referenced in this compilation
        if (requestInterface is null || handlerInterface is null)
            return;

        var requestTypes = new List<INamedTypeSymbol>();
        var handledRequestTypes = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);

        foreach (var type in GetAllTypes(compilation.GlobalNamespace))
        {
            if (type.TypeKind is not (TypeKind.Class or TypeKind.Struct))
                continue;

            if (type.IsAbstract)
                continue;

            foreach (var iface in type.AllInterfaces)
            {
                // Collect request types: class/record implementing IRequest<T>
                if (SymbolEqualityComparer.Default.Equals(iface.OriginalDefinition, requestInterface) &&
                    iface.TypeArguments.Length == 1)
                {
                    requestTypes.Add(type);
                }

                // Collect handled request types from IRequestHandler<TRequest, TResponse>
                if (SymbolEqualityComparer.Default.Equals(iface.OriginalDefinition, handlerInterface) &&
                    iface.TypeArguments.Length == 2 &&
                    iface.TypeArguments[0] is INamedTypeSymbol handledRequest)
                {
                    handledRequestTypes.Add(handledRequest);
                }
            }
        }

        foreach (var requestType in requestTypes)
        {
            if (handledRequestTypes.Contains(requestType))
                continue;

            var location = requestType.Locations.FirstOrDefault(l => l.IsInSource) ?? Location.None;

            var diagnostic = Diagnostic.Create(
                Rule,
                location,
                requestType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));

            context.ReportDiagnostic(diagnostic);
        }
    }

    private static IEnumerable<INamedTypeSymbol> GetAllTypes(INamespaceSymbol root)
    {
        foreach (var type in root.GetTypeMembers())
        {
            foreach (var nested in GetAllTypesRecursive(type))
                yield return nested;
        }

        foreach (var childNs in root.GetNamespaceMembers())
        {
            foreach (var type in GetAllTypes(childNs))
                yield return type;
        }
    }

    private static IEnumerable<INamedTypeSymbol> GetAllTypesRecursive(INamedTypeSymbol type)
    {
        yield return type;

        foreach (var nested in type.GetTypeMembers())
        {
            foreach (var t in GetAllTypesRecursive(nested))
                yield return t;
        }
    }
}