namespace Grove.Core.Decisions
{
  public abstract class SelectCardsPutToBattlefield : SelectCards
  {
    protected override void ProcessCard(Card chosenCard)
    {
      chosenCard.PutToBattlefield();
    }
  }
}