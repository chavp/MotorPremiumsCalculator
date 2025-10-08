using InsuranceProducts.Tests.Domain.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Domain.Products.ValueObjects;

public sealed class Symbol : ValueObject
{
    public const int MaxLength = 5;

    public string Value { get; }

    private Symbol(string value) => Value = value ?? string.Empty;

    /// <summary>
    /// Creates an ItemDescription from a string value.
    /// </summary>
    public static Symbol Create(string value)
    {
        var trimmedValue = value?.Trim() ?? string.Empty;

        if (trimmedValue.Length > MaxLength)
        {
            throw new ArgumentException($"Symbol cannot exceed {MaxLength} characters", nameof(value));
        }

        return new Symbol(trimmedValue);
    }

    /// <summary>
    /// Gets an empty ItemDescription instance.
    /// </summary>
    public static Symbol Empty => new(string.Empty);

    /// <summary>
    /// Checks if the description is empty.
    /// </summary>
    public override bool IsEmpty => string.IsNullOrWhiteSpace(Value);

    /// <summary>
    /// Gets the length of the description.
    /// </summary>
    public int Length => Value.Length;

    public override string ToString() => Value;

    public static implicit operator string(Symbol symbol) => symbol.Value;

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
