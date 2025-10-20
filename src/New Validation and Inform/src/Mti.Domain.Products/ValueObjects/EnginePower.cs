namespace Mti.Domain.Products.ValueObjects;

// Value Object
public record EnginePower
{
    public decimal Value { get; init; }
    public string UnitCode { get; init; } // "HP", "PS", "KW"

    public EnginePower(decimal value, string unitCode)
    {
        if (value <= 0)
            throw new ArgumentException("Engine power must be positive");

        Value = value;
        UnitCode = unitCode.ToUpper();
    }

    // Convert to HP
    public decimal ToHP()
    {
        return UnitCode switch
        {
            "HP" => Value,
            "PS" => Value / 1.014m,
            "KW" => Value * 1.341m,
            "W" => Value / 745.7m,
            _ => throw new NotSupportedException($"Unit {UnitCode} not supported")
        };
    }

    // Convert to kW
    public decimal ToKW()
    {
        return UnitCode switch
        {
            "HP" => Value / 1.341m,
            "PS" => Value / 1.36m,
            "KW" => Value,
            "W" => Value / 1000,
            _ => throw new NotSupportedException($"Unit {UnitCode} not supported")
        };
    }
}
