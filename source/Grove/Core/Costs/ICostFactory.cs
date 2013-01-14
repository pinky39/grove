namespace Grove.Core.Costs
{
  using Grove.Core.Targeting;

  public interface ICostFactory
  {
    Cost CreateCost(Card card, TargetValidator validator, Game game);
  }
}