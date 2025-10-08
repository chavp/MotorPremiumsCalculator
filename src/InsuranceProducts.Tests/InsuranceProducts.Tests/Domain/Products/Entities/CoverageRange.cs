using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Domain.Products.Entities
{
    public sealed class CoverageRange : CoverageLevel
    {
        public decimal LimitFrom { get; private set; }
        public decimal LimitTo { get; private set; }

        protected CoverageRange() { }

        public CoverageRange(Guid id,
            CoverageLevelType type,
            CoverageBasis basis,
            Unit unit,
            decimal limitFrom,
            decimal limitTo) : base(id, type, basis, unit)
        {
            LimitFrom = limitFrom;
            LimitTo = limitTo;
        }
    }
}
