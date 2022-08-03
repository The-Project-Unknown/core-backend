using Microsoft.AspNetCore.Mvc;
using Unknown.DataAccess;

namespace Api.Controllers;

public class FlashCardsController : AbstractFakeCrudController<FlashCard>
{
    private readonly IRepositoryManager _repositoryManager;

    public FlashCardsController(IRepositoryManager repositoryManager) : base(repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }
}

public class OtherCardsController : AbstractFakeCrudController<FlashCard>
{
    private readonly IRepositoryManager _repositoryManager;

    public OtherCardsController(IRepositoryManager repositoryManager) : base(repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }
}