namespace Unknown.DataAccess;

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