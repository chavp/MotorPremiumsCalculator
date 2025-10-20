using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mti.Domain.Products.Tests.Models;

public record ExtCarModel
{
    public string MakeCode { get; set; }
    public string Family { get; set; }
    public string Model { get; set; }
    public string? MotorGroup { get; set; }
    public string? MTIKey { get; set; }
}
