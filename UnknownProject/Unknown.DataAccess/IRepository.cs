namespace Unknown.DataAccess;

public interface ITimeTracked
{
    public DateTime CreationDateTime { get; set; }
    public DateTime UpdateDateTime { get; set; }
}

public abstract class BaseEntity
{
    public long Id { get; set; }
}

public interface IRepository<T> where T : BaseEntity 
{
    T GetById(long id);  
    Task Add(T entity);  
    Task Update(T entity);  
    void Delete(T entity);
    void BulkAdd(IEnumerable<T> entities);
    void BulkUpdate(IEnumerable<T> entities);
    IQueryable<T> AsQueryable { get; }  
}