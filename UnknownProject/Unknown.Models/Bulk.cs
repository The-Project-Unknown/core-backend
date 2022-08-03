using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yieldigo.Models.BulkPricing
{
    public abstract class PrimaryEntity
    {
        public long Id { get; set; }
    }

    public class Bulk : PrimaryEntity
    {
        public long? ArticleId { get; set; }
        public long? LineId { get; set; }
        
        public int Zone { get; set; }
        
        [Column(TypeName = "DATE")] 
        public DateTime? ValidTo { get; set; }
        
        [Column(TypeName = "DATE")] 
        public DateTime CreatedAt { get; set; }
        
        [Column(TypeName = "DATE")] 
        public DateTime? UpdatedAt { get; set; }

        public List<BulkTierPrice> BulkTierPrices { get; set; }
    }
    
    public class Article : PrimaryEntity
    {
        public string Name { get; set; }
        
        public long? LineId { get; set; }
    }
}