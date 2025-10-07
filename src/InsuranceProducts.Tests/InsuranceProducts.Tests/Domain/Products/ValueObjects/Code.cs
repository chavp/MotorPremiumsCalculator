using InsuranceProducts.Tests.Domain.SharedKernel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection.Emit;
using System.Text;

namespace InsuranceProducts.Tests.Domain.Products.ValueObjects
{
    public sealed class Code : ValueObject
    {
        public const int MaxLength = 20;

        public string Value { get; }

        private Code(string value) => Value = value ?? string.Empty;

        /// <summary>
        /// Creates an Code from a string value.
        /// </summary>
        public static Code Create(string value)
        {
            var trimmedValue = value?.Trim() ?? string.Empty;

            if (trimmedValue.Length > MaxLength)
            {
                throw new ArgumentException($"Code cannot exceed {MaxLength} characters", nameof(value));
            }

            var split = trimmedValue.Split(' ');
            if (split.Length > 1)
            {
                throw new ArgumentException("Code cannot contain spaces", nameof(value));
            }

            return new Code(trimmedValue);
        }

        /// <summary>
        /// Gets an empty Code instance.
        /// </summary>
        public static Code Empty => new(string.Empty);

        /// <summary>
        /// Checks if the Code is empty.
        /// </summary>
        public override bool IsEmpty => string.IsNullOrWhiteSpace(Value);

        /// <summary>
        /// Gets the length of the Code.
        /// </summary>
        public int Length => Value.Length;

        public override string ToString() => Value;

        public static implicit operator string(Code code) => code.Value;

        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return Value;
        }

    }
}
