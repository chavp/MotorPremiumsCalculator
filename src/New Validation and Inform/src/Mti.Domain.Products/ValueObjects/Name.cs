using Mti.Domain.SharedKernel.Primatives;

namespace Mti.Domain.Products.ValueObjects;

public sealed class Name : ValueObject
{
    public const int MaxLength = 300;

    public string Value { get; }

    private Name(string value) => Value = value ?? string.Empty;

    /// <summary>
    /// Creates an Name from a string value.
    /// </summary>
    public static Name Create(string value)
    {
        var trimmedValue = value?.Trim() ?? string.Empty;

        if (trimmedValue.Length > MaxLength)
        {
            throw new ArgumentException($"Name cannot exceed {MaxLength} characters", nameof(value));
        }

        return new Name(trimmedValue);
    }

    /// <summary>
    /// Gets an empty ItemDescription instance.
    /// </summary>
    public static Name Empty => new(string.Empty);

    /// <summary>
    /// Checks if the name is empty.
    /// </summary>
    public override bool IsEmpty => string.IsNullOrWhiteSpace(Value);

    /// <summary>
    /// Gets the length of the name.
    /// </summary>
    public int Length => Value.Length;

    public override string ToString() => Value;

    public static implicit operator string(Name description) => description.Value;

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
