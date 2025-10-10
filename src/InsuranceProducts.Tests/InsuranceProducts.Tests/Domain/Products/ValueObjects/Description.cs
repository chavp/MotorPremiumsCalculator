using InsuranceProducts.Tests.Domain.SharedKernel.Primatives;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Domain.Products.ValueObjects
{
    public sealed class Description : ValueObject
    {
        public const int MaxLength = 2000;

        public string Value { get; }

        private Description(string value) => Value = value ?? string.Empty;

        /// <summary>
        /// Creates an ItemDescription from a string value.
        /// </summary>
        public static Description Create(string value)
        {
            var trimmedValue = value?.Trim() ?? string.Empty;

            if (trimmedValue.Length > MaxLength)
            {
                throw new ArgumentException($"Description cannot exceed {MaxLength} characters", nameof(value));
            }

            return new Description(trimmedValue);
        }

        /// <summary>
        /// Gets an empty ItemDescription instance.
        /// </summary>
        public static Description Empty => new(string.Empty);

        /// <summary>
        /// Checks if the description is empty.
        /// </summary>
        public override bool IsEmpty => string.IsNullOrWhiteSpace(Value);

        /// <summary>
        /// Gets the length of the description.
        /// </summary>
        public int Length => Value.Length;

        public override string ToString() => Value;

        public static implicit operator string(Description description) => description.Value;

        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
