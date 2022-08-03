using System.ComponentModel.DataAnnotations;

namespace Yieldigo.Models.BulkPricing
{
    public class BulkBasePrice : PrimaryEntity
    {
        //Todo: kam s tím ?
        public decimal? ExpectedSupplierPrice { get; set; }
        public decimal? SupplierPrice { get; set; }
    }
}