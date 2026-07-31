namespace Interlink;

/// <summary>
/// Specifies the execution order of a pipeline behavior.
/// Behaviors with lower order values execute earlier in the pipeline
/// (closer to the outer edge). Behaviors without this attribute are
/// treated as having <see cref="int.MaxValue"/> order.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class PipelineOrderAttribute : Attribute
{
    /// <summary>
    /// Gets the order value. Lower values run first.
    /// </summary>
    public int Order { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PipelineOrderAttribute"/> class.
    /// </summary>
    /// <param name="order">The order in which the behavior should execute. Lower values run first.</param>
    public PipelineOrderAttribute(int order)
    {
        Order = order;
    }
}