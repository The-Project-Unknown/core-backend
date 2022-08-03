using Microsoft.EntityFrameworkCore;
using Models;
using Yieldigo.Models.BulkPricing;

namespace Unknown.DataAccess;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
        
    }

    public DbSet<FlashCard> FlashCards { get; set; }
}