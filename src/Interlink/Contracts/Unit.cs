namespace Interlink.Contracts;

/// <summary>
/// Represents a void/empty response for commands that do not return a value.
/// Use with <see cref="IRequest"/> or <see cref="IRequest{TResponse}"/> where
/// TResponse is <see cref="Unit"/>.
/// </summary>
public readonly struct Unit : IEquatable<Unit>
{
    /// <summary>
    /// The single <see cref="Unit"/> value.
    /// </summary>
    public static readonly Unit Value = default;

    /// <summary>
    /// Returns a completed task with <see cref="Value"/>.
    /// </summary>
    public static Task<Unit> Task => System.Threading.Tasks.Task.FromResult(Value);

    /// <inheritdoc />
    public bool Equals(Unit other) => true;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Unit;

    /// <inheritdoc />
    public override int GetHashCode() => 0;

    /// <inheritdoc />
    public override string ToString() => "()";

    /// <inheritdoc />
    public static bool operator ==(Unit left, Unit right) => true;

    /// <inheritdoc />
    public static bool operator !=(Unit left, Unit right) => false;
}