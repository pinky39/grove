namespace Grove.Core.Details.Cards.Costs
{
  using Targeting;

  public interface ICostFactory
  {
    Cost CreateCost(Card card, TargetValidator validator);
  }
}