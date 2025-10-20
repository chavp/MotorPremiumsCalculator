using Mti.Domain.SharedKernel.Primatives;

namespace Mti.Domain.Products.ValueObjects;

public sealed class Prefix : ValueObject
{
    public const int MaxLength = 10;

    public string Value { get; }

    private Prefix(string value) => Value = value ?? string.Empty;

    /// <summary>
    /// Creates an Name from a string value.
    /// </summary>
    public static Prefix Create(string value)
    {
        var trimmedValue = value?.Trim() ?? string.Empty;

        if (trimmedValue.Length > MaxLength)
        {
            throw new ArgumentException($"Prefix cannot exceed {MaxLength} characters", nameof(value));
        }

        return new Prefix(trimmedValue);
    }

    /// <summary>
    /// Gets an empty ItemDescription instance.
    /// </summary>
    public static Prefix Empty => new(string.Empty);

    /// <summary>
    /// Checks if the prefix is empty.
    /// </summary>
    public override bool IsEmpty => string.IsNullOrWhiteSpace(Value);

    /// <summary>
    /// Gets the length of the name.
    /// </summary>
    public int Length => Value.Length;

    public override string ToString() => Value;

    public static implicit operator string(Prefix prefix) => prefix.Value;

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
