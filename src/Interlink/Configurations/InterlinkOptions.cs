namespace Interlink;

/// <summary>
/// Provides configuration options for Interlink services.
/// </summary>
public class InterlinkOptions
{
    internal List<(Type Type, int? Order)> OpenBehaviors { get; } = new();

    /// <summary>
    /// Gets or sets an optional custom factory used to resolve handlers and pipeline components.
    /// When set, this factory is preferred over the default <see cref="IServiceProvider"/>.
    /// </summary>
    public Func<Type, object?>? ServiceFactory { get; set; }

    /// <summary>
    /// Adds an open-generic pipeline behavior type to the configuration.
    /// </summary>
    /// <param name="openGenericBehaviorType">
    /// An open generic type that implements <see cref="IPipelineBehavior{TRequest,TResponse}"/>
    /// (for example <c>typeof(LoggingBehavior&lt;,&gt;)</c>).
    /// </param>
    /// <param name="order">
    /// Optional explicit order. When provided, takes precedence over any
    /// <see cref="PipelineOrderAttribute"/> on the type. Lower values run first.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="openGenericBehaviorType"/> is null.</exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="openGenericBehaviorType"/> is not an open generic type with exactly two type parameters.
    /// </exception>
    public void AddBehavior(Type openGenericBehaviorType, int? order = null)
    {
        if (openGenericBehaviorType is null)
            throw new ArgumentNullException(nameof(openGenericBehaviorType));

        if (!openGenericBehaviorType.IsGenericTypeDefinition ||
            openGenericBehaviorType.GetGenericArguments().Length != 2)
        {
            throw new ArgumentException(
                "Behavior must be an open generic type definition with exactly two generic parameters " +
                "(for example typeof(MyBehavior<,>)).",
                nameof(openGenericBehaviorType));
        }

        OpenBehaviors.Add((openGenericBehaviorType, order));
    }

    /// <summary>
    /// Adds an open-generic pipeline behavior type to the configuration using a generic type parameter.
    /// </summary>
    /// <typeparam name="TBehavior">
    /// The open generic behavior type that implements <see cref="IPipelineBehavior{TRequest,TResponse}"/>.
    /// </typeparam>
    /// <param name="order">
    /// Optional explicit order. When provided, takes precedence over any
    /// <see cref="PipelineOrderAttribute"/> on the type. Lower values run first.
    /// </param>
    public void AddBehavior<TBehavior>(int? order = null)
        where TBehavior : class
    {
        AddBehavior(typeof(TBehavior), order);
    }
}