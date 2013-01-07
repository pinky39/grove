namespace Grove.Core.Cards.Casting
{
  public interface ICastingRuleFactory
  {
    CastingRule CreateCastingRule(Card card, Game game);
  }
}