using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Infrastructure.Persistence.Configurations
{
    public static class ValueConverters
    {
        public static ValueConverter<string, string> UpperConverter =>
            new ValueConverter<string, string>(
                v => v.ToUpperInvariant(),
                v => v.ToUpperInvariant());

        public static ValueConverter<string, string> LowerConverter =>
            new ValueConverter<string, string>(
                v => v.ToLowerInvariant(),
                v => v.ToLowerInvariant());
    }
}
