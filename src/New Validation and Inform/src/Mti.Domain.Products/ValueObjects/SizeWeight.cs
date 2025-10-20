namespace Mti.Domain.Products.ValueObjects;

public record SizeWeight
{
    public decimal Value { get; init; }
    public string UnitCode { get; init; } // "HP", "PS", "KW"

    public SizeWeight(decimal value, string unitCode)
    {
        if (value <= 0)
            throw new ArgumentException("Size weight must be positive");

        Value = value;
        UnitCode = unitCode.ToUpper();
    }

    // Convert to HP
    public decimal ToKG()
    {
        return UnitCode switch
        {
            "KG" => Value,
            "TON" => Value / 1000m,
            _ => throw new NotSupportedException($"Unit {UnitCode} not supported")
        };
    }

    // Convert to kW
    public decimal ToTon()
    {
        return UnitCode switch
        {
            "TON" => Value,
            "KG" => Value * 1000m,
            _ => throw new NotSupportedException($"Unit {UnitCode} not supported")
        };
    }
}
