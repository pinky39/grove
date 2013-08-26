namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class Annul : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
              .Is.CounterableSpell(e => e.Source.OwningCard.Is().Artifact || e.Source.OwningCard.Is().Enchantment)
              .On.Stack());

            p.TargetingRule(new EffectCounterspell());
            p.TimingRule(new WhenTopSpellIsCounterable());
          });
    }
  }
}