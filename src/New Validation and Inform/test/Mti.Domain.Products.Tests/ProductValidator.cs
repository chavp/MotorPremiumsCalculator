using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mti.Domain.Products.Tests
{
    public class ProductValidator
    {

        [Fact]
        public void PreProcessing()
        {
            var productData = new ProductData();

            if(string.IsNullOrEmpty(productData.TaskType))
            {

            }
            if (string.IsNullOrEmpty(productData.PolicyType))
            {

            }
            if (!productData.EffectiveDate.HasValue)
            {

            }
            if (!productData.ExpiryDate.HasValue)
            {

            }
        }
    }

    public record MotorInformData
    {
        
    }

    public record SaleOrderData
    {
        public string? ReferenceNo { get; set; }
        public DateOnly SaleDate { get; set; }
        public string? AgentNo { get; set; }
    }

    public record ProductData
    {
        public string? TaskType { get; set; }
        public string? PolicyType { get; set; }
        public DateOnly? EffectiveDate { get; set; }
        public DateOnly? ExpiryDate { get; set; }

        public string? Campaign { get; set; }
        public string? Package { get; set; }

    }

    public record PartyData
    {
        public string? Role { get; set; }

    }
}
