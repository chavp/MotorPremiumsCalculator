using EnsureThat;
using InsuranceProducts.Tests.Domain.SharedKernel;
using InsuranceProducts.Tests.Domain.SharedKernel.Primatives;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Domain.Products.Entities
{
    public class CoverageAvailability : Entity<Guid>
    {
        public Guid? ProductId { get; protected set; }
        public Product? Product { get; protected set; }

        public Guid? CoverageAvailabilityTypeId { get; protected set; }
        public CoverageAvailabilityType? CoverageAvailabilityType { get; protected set; }

        public Guid? CoverageTypeId { get; protected set; }
        public CoverageType? CoverageType { get; protected set; }

        public Guid? CoverageLevelId { get; protected set; }
        public CoverageLevel? CoverageLevel { get; protected set; }

        protected CoverageAvailability() { }

        public CoverageAvailability(
            Guid id,
            Product product,
            CoverageAvailabilityType type,
            CoverageType coverageType,
            CoverageLevel coverageLevel) : base(id)
        {
            Id = Ensure.That(id).NotEmpty("Id is not empty", nameof(id));
            Product = product;
            CoverageAvailabilityType = type;
            CoverageType = coverageType;
            CoverageLevel = coverageLevel;
        }
    }
}
