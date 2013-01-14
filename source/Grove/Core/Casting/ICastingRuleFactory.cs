namespace Grove.Core.Casting
{
  public interface ICastingRuleFactory
  {
    CastingRule CreateCastingRule(Card card, Game game);
  }
}