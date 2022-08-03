namespace Unknown.DataAccess;

public interface ITimeTracked
{
    public DateTime CreationDateTime { get; set; }
    public DateTime UpdateDateTime { get; set; }
}