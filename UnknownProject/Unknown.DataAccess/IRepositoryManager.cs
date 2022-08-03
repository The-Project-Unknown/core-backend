namespace Unknown.DataAccess;

public interface IRepositoryManager
{
    public ApiDbContext Context { get; }
    public IFlashCardRepository FlashCardsRepo { get; set; }
}