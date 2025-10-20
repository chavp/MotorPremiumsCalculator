using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mti.Domain.Products.Tests.Models;

public record TB_M_CONFIG
{
    public string? Code { get; init; }
    public string? Description { get; set; }

    /// <summary>
    /// policy type
    /// </summary>
    public string? Value1 { get; init; }

    /// <summary>
    /// campaign
    /// </summary>
    public string? Value3 { get; init; }

    public decimal Value4 { get; init; }
    public decimal Value5 { get; init; }
    public decimal Value6 { get; init; }
    public decimal Value7 { get; init; }
    public decimal Value8 { get; init; }
    public decimal Value9 { get; init; }
    public decimal Value12 { get; init; }
    /// <summary>
    /// package
    /// </summary>
    public string? Value11 { get; init; }

    public string? Value10 { get; init; }

    public string? Remark { get; init; }

    public DateTime Start_Date { get; init; }
    public DateTime End_Date { get; init; }

    public string? Value14 { get; init; }
    public string? Value18 { get; init; }

    public decimal net_premium { get; init; }

    public string? Value15 { get; init; }

    public string? Value16 { get; init; }
}
