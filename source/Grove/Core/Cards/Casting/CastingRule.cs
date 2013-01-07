namespace Grove.Core.Cards.Casting
{
  using Dsl;
  using Effects;
  using Infrastructure;
  using Zones;

  [Copyable]
  public abstract class CastingRule 
  {
    protected Card Card { get; private set; }
    protected Game Game { get; private set; }    
    
    public abstract bool CanCast();
    public abstract void Cast(Effect effect);
    public abstract void AfterResolve();
    

      public class Factory<TCastingRule> : ICastingRuleFactory where TCastingRule : CastingRule, new()
    {
      public Initializer<TCastingRule> Init = delegate { };

      public CastingRule CreateCastingRule(Card card, Game game)
      {
        var rule = new TCastingRule();
        rule.Card = card;
        rule.Game = game;
       
        Init(rule);        

        return rule;
      }
    }
  }
}