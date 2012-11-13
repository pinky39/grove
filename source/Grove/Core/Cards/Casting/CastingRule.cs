namespace Grove.Core.Cards.Casting
{
  using Effects;
  using Grove.Infrastructure;

  [Copyable]
  public abstract class CastingRule
  {
    public abstract bool CanCast(Card card);
    public abstract void Cast(Effect effect);    
  }
}