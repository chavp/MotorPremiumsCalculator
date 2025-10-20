using EnsureThat;
using Mti.Domain.Products.Extensions;
using Mti.Domain.Products.ValueObjects;
using Mti.Domain.SharedKernel.Primatives;

namespace Mti.Domain.Products.Entities
{
    public sealed class VehicleTypeVoluntary : Entity<Guid>
    {
        public Code Code { get; protected set; }
        public Name Name { get; protected set; }
        public Description Description { get; protected set; }

        public Guid VehicleFuelTypeId { get; protected set; }
        public VehicleFuelType VehicleFuelType { get; protected set; }

        public Guid VehicleUsageId { get; protected set; }
        public VehicleUsage VehicleUsage { get; protected set; }

        public Guid VehicleTypeId { get; protected set; }
        public VehicleType VehicleType { get; protected set; }

        public List<VehicleSize> VehicleSizes { get; protected set; } = [];
        public List<VehicleTypeCompulsory> VehicleTypeCompulsories { get; protected set; } = [];

        protected VehicleTypeVoluntary(Builder builder) : base(builder.Id)
        {
            Id = builder.Id;
            Code = builder.Code;
            VehicleFuelType = builder.VehicleFuelType;
            VehicleType = builder.VehicleType;
            VehicleUsage = builder.VehicleUsage;

            VehicleSizes = builder.VehicleSizes.ToList();
            VehicleTypeCompulsories = builder.VehicleTypeCompulsories.ToList();
        }

        public VehicleTypeVoluntary() : base()
        {
            Code = default!;
        }

        public VehicleTypeVoluntary UpdateName(string name)
        {
            Name = Name.Create(name);
            return this;
        }

        public VehicleTypeVoluntary UpdateDescription(string description)
        {
            Description = Description.Create(description);
            return this;
        }

        //public static Builder CreateBuilder(VehicleFuelType vehicleFuelType, VehicleType vehicleType, VehicleUsage vehicleUsage)
        //{
        //    var code = $"{vehicleFuelType.Prefix}{vehicleType.Code}{vehicleUsage.Code}".Trim();
        //    var builder = new Builder(Guid.NewGuid(), Code.Create(code),
        //        vehicleFuelType, vehicleType, vehicleUsage);
        //    return builder;
        //}

        public static Builder CreateBuilder(VehicleFuelType vehicleFuelType, VehicleType vehicleType)
        {
            var code = $"{vehicleFuelType.Prefix}{vehicleType.Code}".Trim();
            var builder = new Builder(vehicleFuelType, vehicleType);
            return builder;
        }

        public static string ConvertCode(string code)
        {
            var vehicleTypeVoluntaryCode = code;
            if (vehicleTypeVoluntaryCode.EndsWith("E"))
            {
                vehicleTypeVoluntaryCode = $"E{vehicleTypeVoluntaryCode.Substring(0, 2)}";
            }
            return vehicleTypeVoluntaryCode;
        }

        public sealed class Builder
        {
            internal Guid Id { get; set; } = default!;
            internal Code Code { get; set; } = default!;
            internal Name Name { get; set; } = default!;
            internal Description Description { get; set; } = default!;

            internal VehicleFuelType VehicleFuelType { get; set; }
            internal VehicleType VehicleType { get; set; }
            internal VehicleUsage VehicleUsage { get; set; }

            internal List<VehicleSize> VehicleSizes { get; set; } = [];
            internal List<VehicleTypeCompulsory> VehicleTypeCompulsories { get; set; } = [];

            private List<VehicleTypeVoluntary> _VehicleTypeVoluntaries = [];

            public Builder WithName(string name)
            {
                Name = Name.Create(name);
                return this;
            }

            public Builder WithDescription(string description)
            {
                Description = Description.Create(description);
                return this;
            }

            public Builder WithUsage(VehicleUsage usage)
            {
                VehicleUsage = usage;

                // reset
                VehicleSizes = [];
                VehicleTypeCompulsories = [];
                Code = default;
                Description = default;
                Name = default;

                return this;
            }

            public Builder AddSize(decimal min, decimal max, Unit unit)
            {
                var size = new VehicleSize(min, max, unit);
                VehicleSizes.Add(size);
                return this;
            }

            public Builder AddCompulsory(string code, string? description = null)
            {
                var comp = new VehicleTypeCompulsory(code, description);
                VehicleTypeCompulsories.Add(comp);
                return this;
            }
            public Builder AddCompulsories(params string[] codes)
            {
                foreach (var code in codes)
                {
                    var comp = new VehicleTypeCompulsory(code, string.Empty);
                    VehicleTypeCompulsories.Add(comp);
                }
                return this;
            }

            internal Builder(Guid id, Code code,
                VehicleFuelType vehicleFuelType, VehicleType vehicleType, VehicleUsage vehicleUsage)
            {
                Id = Ensure.That(id).NotEmpty("Id is not empty", nameof(id));
                Code = Ensure.That(code).NotEmpty("Code is not empty", nameof(code));
                Ensure.That(vehicleFuelType).IsNotNull();
                Ensure.That(vehicleType).IsNotNull();
                Ensure.That(vehicleUsage).IsNotNull();

                VehicleFuelType = vehicleFuelType;
                VehicleType = vehicleType;
                VehicleUsage = vehicleUsage;
            }

            internal Builder(VehicleFuelType vehicleFuelType, VehicleType vehicleType)
            {
                Ensure.That(vehicleFuelType).IsNotNull();
                Ensure.That(vehicleType).IsNotNull();

                VehicleFuelType = vehicleFuelType;
                VehicleType = vehicleType;
            }

            public VehicleTypeVoluntary Build()
            {
                Ensure.That(VehicleUsage).IsNotNull();

                Id = Guid.NewGuid();
                Code = Code.Create($"{VehicleFuelType.Prefix}{VehicleType.Code}{VehicleUsage.Code}".Trim());
                
                var newEntity = new VehicleTypeVoluntary(this);
                if (Description != null && !Description.IsEmpty)
                {
                    newEntity.UpdateDescription(Description);
                }
                if (Name != null && !Name.IsEmpty)
                {
                    newEntity.UpdateName(Name);
                }

                return newEntity;
            }

            public Builder Next()
            {
                var item = Build();
                _VehicleTypeVoluntaries.Add(item);
                return this;
            }

            public IReadOnlyList<VehicleTypeVoluntary> End()
            {
                var copy = _VehicleTypeVoluntaries.ToList();
                _VehicleTypeVoluntaries.Clear();
                return copy;
            }
        }
    }
}
