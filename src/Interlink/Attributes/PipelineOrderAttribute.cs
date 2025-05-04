namespace Interlink;

/// <summary>
/// Represents an attribute that specifies the order of a pipeline behavior.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class PipelineOrderAttribute : Attribute
{
    /// <summary>
    /// Gets the order of the pipeline behavior.
    /// </summary>
    public int Order { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PipelineOrderAttribute"/> class with the specified order.
    /// </summary>
    /// <param name="order"></param>
    public PipelineOrderAttribute(int order)
    {
        Order = order;
    }
}
