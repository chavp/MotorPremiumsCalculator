using EnsureThat;
using InsuranceProducts.Tests.Domain.SharedKernel.Primatives;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Domain.Products.Entities
{
    public sealed class CoverageTypeComposition : Entity<Guid>
    {
        public Guid? FromCoverageTypeId { get; private set; }
        public CoverageType? FromCoverageType { get; private set; }

        public Guid? ToCoverageTypeId { get; private set; }
        public CoverageType? ToCoverageType { get; private set; }

        protected CoverageTypeComposition() { }

        public CoverageTypeComposition(
            CoverageType from,
            CoverageType to)
        {
            Id = Guid.NewGuid();
            FromCoverageType = from;
            ToCoverageType = to;
        }
    }
}
