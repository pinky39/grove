namespace Grove.Core.DamagePrevention
{
  public interface IDamagePreventionFactory
  {
    DamagePrevention Create(Card card);
  }
}