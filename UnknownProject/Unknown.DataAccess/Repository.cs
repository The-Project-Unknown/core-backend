using Microsoft.EntityFrameworkCore;

namespace Unknown.DataAccess;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly ApiDbContext _context;
    public IQueryable<T> AsQueryable => _context.Set<T>().AsQueryable();

    //TODO: Dont be lazy bitch and implement it
    
    public Repository(ApiDbContext context)
    {
        _context = context;
    }
    
    public T GetById(long id)
    {
        throw new NotImplementedException();
    }

    public async Task Add(T entity)
    {
        throw new NotImplementedException();
    }

    public async Task Update(T entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(T entity)
    {
        throw new NotImplementedException();
    }

    public void BulkAdd(IEnumerable<T> entities)
    {
        throw new NotImplementedException();
    }

    public void BulkUpdate(IEnumerable<T> entities)
    {
        throw new NotImplementedException();
    }

}
