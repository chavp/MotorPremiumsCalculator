using EnsureThat;
using InsuranceProducts.Tests.Domain.Products.ValueObjects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Domain.SharedKernel;

public static class EnsureArgumentNullExtensions
{
    public static Code NotEmpty(this Param<Code> param,
        string message, string paramName)
    {
        param.IsNotNull();
        return !param.Value.IsEmpty
            ? param.Value
            : throw Ensure.ExceptionFactory.ArgumentNullException(message, paramName);
    }

    public static Guid NotEmpty(this Param<Guid> param,
        string message, string paramName)
        => param.Value != Guid.Empty
               ? param.Value
               : throw Ensure.ExceptionFactory.ArgumentNullException(message, paramName);
}
