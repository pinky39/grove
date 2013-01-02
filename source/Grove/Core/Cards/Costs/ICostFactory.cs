namespace Grove.Core.Cards.Costs
{
  using Targeting;

  public interface ICostFactory
  {
    Cost CreateCost(Card card, TargetValidator validator, Game game);
  }
}