namespace Grove.Core.Costs
{
  public interface ICostFactory
  {
    Cost CreateCost(ActivatedAbility ability);
  }
}