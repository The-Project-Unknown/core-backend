using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yieldigo.Models.BulkPricing
{
    public class BulkOffer : PrimaryEntity
    {
        public long ArticleId { get; set; }
        
        //public Article Article { get; set; }
        
        public long ZoneId { get; set; }
        
        //public Zone Zone { get; set; }
        
        public decimal Tier { get; set; }
        
        [Column(TypeName = "DATE")] 
        public DateTime DateFrom { get; set; }
        
        [Column(TypeName = "DATE")] 
        public DateTime DateTo { get; set; }
        
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        
        public string Color { get; set; }
    }
}