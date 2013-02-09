namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Effects;

  public class Annul : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Annul")
        .ManaCost("{U}")
        .Type("Instant")
        .Text("Counter target artifact or enchantment spell.")
        .FlavorText("The most effective way to destroy a spell is to ensure it was never cast in the first place.")
        .Cast(p =>
          {
            p.Effect = () => new CounterTargetSpell {Category = EffectCategories.Counterspell};
            p.TargetSelector.AddEffect(t => t
              .Is.Counterable(card => card.Is().Artifact || card.Is().Enchantment)
              .On.Stack());
            
            p.TargetingRule(new Core.Ai.TargetingRules.Counterspell());
            p.TimingRule(new Core.Ai.TimingRules.Counterspell());
          });
    }
  }
}