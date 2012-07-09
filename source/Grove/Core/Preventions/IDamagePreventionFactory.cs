namespace Grove.Core.Preventions
{
  public interface IDamagePreventionFactory
  {
    DamagePrevention Create(ITarget owner);
  }
}