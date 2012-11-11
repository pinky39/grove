namespace Grove.Core.Details.Cards.Casting
{
  using Effects;
  using Infrastructure;

  [Copyable]
  public abstract class CastingRule
  {
    public abstract bool CanCast(Card card);
    public abstract void Cast(Effect effect);    
  }
}