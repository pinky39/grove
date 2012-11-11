namespace Grove.Core.Details.Cards.Preventions
{
  using Targeting;

  public interface IDamagePreventionFactory
  {
    DamagePrevention Create(ITarget preventionOwner, Game game);
  }
}