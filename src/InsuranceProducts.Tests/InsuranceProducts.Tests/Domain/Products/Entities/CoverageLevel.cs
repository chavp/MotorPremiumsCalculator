using EnsureThat;
using InsuranceProducts.Tests.Domain.Products.ValueObjects;
using InsuranceProducts.Tests.Domain.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace InsuranceProducts.Tests.Domain.Products.Entities
{
    public abstract class CoverageLevel : Entity<Guid>
    {
        public CoverageLevelType? CoverageLevelType { get; protected set; }
        public CoverageBasis? CoverageBasis { get; protected set; }
        public Unit? Unit { get; protected set; }

        protected CoverageLevel() { }

        protected CoverageLevel(
            Guid id,
            CoverageLevelType type,
            CoverageBasis basis,
            Unit unit) : base(id)
        {
            Id = Ensure.That(id).NotEmpty("Id is not empty", nameof(id));
            CoverageLevelType = type;
            CoverageBasis = basis;
            Unit = unit;
        }

        public static Builder CreateBuilder() => new Builder();

        public sealed class Builder
        {
            internal CoverageLevelType? CoverageLevelType { get; private set; }
            internal CoverageBasis? CoverageBasis { get; private set; }
            internal Unit? Unit { get; private set; }

            private decimal _amount;
            private decimal _minAmount;
            private decimal _maxAmount;

            public Builder WithCoverageLevelType(CoverageLevelType? coverageLevelType)
            {
                CoverageLevelType = coverageLevelType;
                return this;
            }

            public Builder WithAmount(decimal amount)
            {
                Ensure.That(CoverageLevelType.Code.Value).IsEqualTo(CoverageLevelType.CoverageAmount);
                Ensure.That(amount).IsGt(0);
                _amount = amount;
                return this;
            }
            public Builder WithLimit(decimal amount)
            {
                Ensure.That(CoverageLevelType.Code.Value).IsEqualTo(CoverageLevelType.CoverageLimit);
                Ensure.That(amount).IsGt(0);
                _amount = amount;
                return this;
            }
            public Builder WithRange(decimal minimumAmount, decimal maximumAmount)
            {
                Ensure.That(CoverageLevelType.Code.Value).IsEqualTo(CoverageLevelType.CoverageRange);
                Ensure.That(minimumAmount).IsGt(0);
                Ensure.That(maximumAmount).IsGt(0);
                Ensure.That(maximumAmount).IsGte(minimumAmount);
                _minAmount = minimumAmount;
                _maxAmount = maximumAmount;
                return this;
            }

            public Builder WithCoverageBasis(CoverageBasis basis)
            {
                Ensure.That(basis).IsNotNull();
                CoverageBasis = basis;
                return this;
            }

            public Builder WithUnit(Unit unit)
            {
                Ensure.That(unit).IsNotNull();
                Unit = unit;
                return this;
            }

            public Builder()
            {

            }

            public CoverageLevel Build()
            {
                CoverageLevel newEntity = CoverageLevelType.Code.Value switch
                {
                    CoverageLevelType.CoverageAmount => new CoverageAmount(
                                            Guid.NewGuid(),
                                            CoverageLevelType,
                                            CoverageBasis,
                                            Unit)
                                       .SetAmount(_amount),
                    CoverageLevelType.CoverageLimit => new CoverageLimit(
                                            Guid.NewGuid(),
                                            CoverageLevelType,
                                            CoverageBasis,
                                            Unit
                                        )
                                        .SetAmount(_amount),
                    CoverageLevelType.CoverageRange => new CoverageRange(
                                            Guid.NewGuid(),
                                            CoverageLevelType,
                                            CoverageBasis,
                                            Unit
                                         )
                                         .SetRange(_minAmount, _maxAmount),
                    _ => throw new ArgumentException("Invalid coverage level type", nameof(CoverageLevelType.Code))
                };

                return newEntity;
            }
        }
    }
}
