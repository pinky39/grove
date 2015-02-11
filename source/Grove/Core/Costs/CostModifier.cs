namespace Grove.Costs
{
  public abstract class CostModifier : GameObject
  {
    private readonly int _amount;
    protected Card Source;

    protected CostModifier() {}

    protected CostModifier(int amount)
    {
      _amount = amount;
    }

    public void Initialize(Card source, Game game)
    {
      Game = game;
      Source = source;
    }

    protected virtual bool ShouldApply(Card card, CostType type)
    {
      return true;
    }

    public virtual int GetChange(CostType type, Card card)
    {
      return ShouldApply(card, type) ? _amount : 0;
    }
  }
}