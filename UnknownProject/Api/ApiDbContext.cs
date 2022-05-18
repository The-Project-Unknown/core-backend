using Microsoft.EntityFrameworkCore;
using Models;

namespace Api;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
        
    }

    public virtual DbSet<TestingClassForDbContext> TestingClassForDbContext { get; set; }
}