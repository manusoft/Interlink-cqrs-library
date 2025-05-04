namespace Interlink;

/// <summary>
/// Represents options for configuring Interlink.
/// </summary>
public class InterlinkOptions
{
    internal List<Type> OpenBehaviors { get; } = new();

    /// <summary>
    /// Adds a pipeline behavior to the configuration.
    /// </summary>
    /// <param name="openGenericBehaviorType">The open generic type of the pipeline behavior.</param>
    /// <exception cref="ArgumentException"></exception>
    public void AddBehavior(Type openGenericBehaviorType)
    {
        if (!openGenericBehaviorType.IsGenericTypeDefinition ||
            openGenericBehaviorType.GetGenericArguments().Length != 2)
        {
            throw new ArgumentException("Behavior must be an open generic type with two generic parameters.", nameof(openGenericBehaviorType));
        }

        OpenBehaviors.Add(openGenericBehaviorType);
    }

    /// <summary>
    /// Adds a pipeline behavior to the configuration using a generic type.
    /// </summary>
    /// <typeparam name="T">The type of the pipeline behavior.</typeparam>
    public void AddBehavior<T>() where T : class
    {
        AddBehavior(typeof(T));
    }
}