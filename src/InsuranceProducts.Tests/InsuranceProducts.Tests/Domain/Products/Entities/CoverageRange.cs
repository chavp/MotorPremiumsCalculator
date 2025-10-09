using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Domain.Products.Entities
{
    public sealed class CoverageRange : CoverageLevel
    {
        public decimal MinimumAmount { get; private set; }
        public decimal MaximumAmount { get; private set; }

        public CoverageRange SetRange(decimal minimumAmount, decimal maximumAmount)
        {
            MinimumAmount = minimumAmount;
            MaximumAmount = maximumAmount;
            return this;
        }

        protected CoverageRange() { }

        protected internal CoverageRange(Guid id,
            CoverageLevelType type,
            CoverageBasis basis,
            Unit unit) : base(id, type, basis, unit)
        {
        }
    }
}
