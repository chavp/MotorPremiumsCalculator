using EnsureThat;
using Mti.Domain.Products.Extensions;
using Mti.Domain.Products.ValueObjects;
using Mti.Domain.SharedKernel.Primatives;

namespace Mti.Domain.Products.Entities;

public sealed class VehicleTypeCompulsory : Entity<Guid>
{
    public Guid? VehicleTypeVoluntaryId { get; set; }
    public VehicleTypeVoluntary? VehicleTypeVoluntary { get; set; }

    public Code Code { get; private set; }
    public Description Description { get; private set; }

    protected VehicleTypeCompulsory() { }

    public VehicleTypeCompulsory(string code, string description)
    {
        Code = Code.Create(code);
        Description = Description.Create(description);
    }

}
