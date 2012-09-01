namespace Grove.Core.Controllers.Machine
{
  using System.Linq;

  public class ReturnCardsFromGraveyardToBattlefield : Controllers.ReturnCardsFromGraveyardToBattlefield
  {
    protected override void ExecuteQuery()
    {
      var cards = Controller.Graveyard
        .Where(Filter)
        .OrderByDescending(x => x.Score)
        .Take(Count);

      Result = cards.ToList();
    }
  }
}