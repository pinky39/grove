namespace Grove.Core.Controllers
{
  public abstract class SelectCardsPutToBattlefield : SelectCards
  {
    protected override void ProcessCard(Card chosenCard)
    {
      chosenCard.PutToBattlefield();
    }
  }
}