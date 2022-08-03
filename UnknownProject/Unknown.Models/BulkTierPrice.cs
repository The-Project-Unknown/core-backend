namespace Yieldigo.Models.BulkPricing
{
    public class BulkTierPrice : PrimaryEntity
    {
        public long BulkId { get; set; }
        
        public Bulk Bulk { get; set; }
        
        public int Tier { get; set; }

        //public decimal? LineAmount { get; set; }

        public int DiscountType { get; set; }
        
        public decimal DiscountValue { get; set; }

        public decimal Price { get; set; }
    }
}