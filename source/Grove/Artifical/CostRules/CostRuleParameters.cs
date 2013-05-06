namespace Grove.Artifical.CostRules
{
  using Gameplay;
  using Gameplay.Targeting;

  public class CostRuleParameters
  {
    public CostRuleParameters(Card owningCard, int maxX, Targets targets = null)
    {
      OwningCard = owningCard;
      MaxX = maxX;
      Targets = targets;
    }

    public Card OwningCard { get; private set; }
    public int MaxX { get; private set; }
    public Targets Targets { get; private set; }
    public Player Controller { get { return OwningCard.Controller; } }
  }
}