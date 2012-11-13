namespace Grove.Core.Decisions
{
  public abstract class SelectCardsReturnToHand : SelectCards
  {
    protected override void ProcessCard(Card chosenCard)
    {
      chosenCard.ReturnToHand();
    }
  }
}