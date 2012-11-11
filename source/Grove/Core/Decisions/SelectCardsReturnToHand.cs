namespace Grove.Core.Controllers
{
  public abstract class SelectCardsReturnToHand : SelectCards
  {
    protected override void ProcessCard(Card chosenCard)
    {
      chosenCard.ReturnToHand();
    }
  }
}