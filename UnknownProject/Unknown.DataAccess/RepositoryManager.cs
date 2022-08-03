using Microsoft.EntityFrameworkCore;

namespace Unknown.DataAccess;

public class RepositoryManager : IRepositoryManager
{
    public ApiDbContext Context { get; }
    public IFlashCardRepository FlashCardsRepo { get; set; }
    
    public RepositoryManager(ApiDbContext context, IFlashCardRepository flashCardsRepo)
    {
        Context = context;
        FlashCardsRepo = flashCardsRepo;
    }
}