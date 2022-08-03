namespace Unknown.DataAccess;

public class FlashCardsRepository : Repository<FlashCard>, IFlashCardRepository
{
    public FlashCardsRepository(ApiDbContext context) : base(context)
    {
        
    }
}