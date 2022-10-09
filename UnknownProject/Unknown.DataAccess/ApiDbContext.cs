
using Microsoft.EntityFrameworkCore;

namespace Unknown.DataAccess;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
        
    }

    public DbSet<FlashCard> FlashCards { get; set; }
}