using Mti.Domain.SharedKernel.Primatives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mti.Domain.Products.Entities;

public sealed class VehicleSize : Entity<Guid>
{
    public Guid? VehicleTypeVoluntaryId { get; set; }
    public VehicleTypeVoluntary? VehicleTypeVoluntary { get; set; }

    public decimal Min { get; set; }
    public decimal Max { get; set; }

    public Guid? UnitId { get; protected set; }
    public Unit? Unit { get; protected set; }

    protected VehicleSize() { }

    public VehicleSize(decimal min, decimal max, Unit unit)
    {
        Min = min; 
        Max = max; 
        Unit = unit;
    }
}
