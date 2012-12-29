namespace Grove.Core.Decisions
{
  using Zones;

  public abstract class SelectCardsToUntap : SelectCards
  {
    protected SelectCardsToUntap()
    {
      Zone = Zone.Battlefield;
    }
    
    protected override void ProcessCard(Card chosenCard)
    {
      chosenCard.Untap();
    }
  }
}