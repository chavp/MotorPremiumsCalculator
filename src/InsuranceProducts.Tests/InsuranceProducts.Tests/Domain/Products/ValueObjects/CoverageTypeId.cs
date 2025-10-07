using EnsureThat;
using InsuranceProducts.Tests.Domain.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Domain.Products.ValueObjects;

/// <summary>
/// Value object representing a unique identifier for a Product Backlog.
/// </summary>
public sealed class CoverageTypeId : ValueObject
{
    public Guid Value { get; }

    private CoverageTypeId(Guid value)
    {
        Value = Ensure
            .That(value)
            .NotEmpty("CoverageTypeId cannot be empty", nameof(value));
    }

    /// <summary>
    /// Creates a new CoverageTypeId with a unique identifier.
    /// </summary>
    public static CoverageTypeId New() => new(Guid.NewGuid());

    /// <summary>
    /// Creates a CoverageTypeId from an existing Guid value.
    /// </summary>
    public static CoverageTypeId From(Guid value) => new(value);

    /// <summary>
    /// Creates a CoverageTypeId from a string representation.
    /// </summary>
    public static CoverageTypeId From(string value)
    {
        if (!Guid.TryParse(value, out var guid))
        {
            throw new ArgumentException("Invalid CoverageTypeId format", nameof(value));
        }
        return From(guid);
    }

    public override string ToString() => Value.ToString();

    public override bool IsEmpty => Value == Guid.Empty;

    public static implicit operator Guid(CoverageTypeId id) => id.Value;

    public static implicit operator string(CoverageTypeId id) => id.Value.ToString();

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
