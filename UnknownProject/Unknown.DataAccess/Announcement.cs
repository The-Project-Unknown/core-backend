using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Yieldigo.Models.BulkPricing;

namespace Unknown.DataAccess;

public class Announcement : PrimaryEntity
{
    [NotMapped, JsonIgnore]
    public static readonly string AllSchemas = "AllSchemas";
    
    [NotMapped]
    public string Message { get; set; }
    
    /// <summary>
    /// Critical - red
    /// Update - green
    /// General - blue
    /// </summary>
    public string AnnouncementType { get; set; }
    
    /// <summary>
    /// Since when it should be displayed
    /// </summary>
    public DateTime DisplayFrom { get; set; }
    
    /// <summary>
    /// Till when it should be displayed
    /// </summary>
    public DateTime DisplayTo { get; set; }
    
    /// <summary>
    /// Date and time that will be displayed in announcement message
    /// Time-from
    /// </summary>
    public DateTime? EventDateFrom { get; set; }
    
    /// <summary>
    /// Date and time that will be displayed in announcement message
    /// Time-to
    /// </summary>
    public DateTime? EventDateTo { get; set; }

}