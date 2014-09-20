namespace Grove.Costs
{
  public abstract class CostModifier : GameObject
  {
    protected Card Source;
    public abstract IManaAmount GetActualCost(IManaAmount amount, ManaUsage manaUsage, Card card);

    public void Initialize(Card source, Game game)
    {
      Game = game;
      Source = source;
    }
  }
}