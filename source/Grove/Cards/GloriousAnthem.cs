namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class GloriousAnthem : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Glorious Anthem")
        .ManaCost("{1}{W}{W}")
        .Type("Enchantment")
        .Text("Creatures you control get +1/+1.")
        .FlavorText("Once heard, the battle song of an angel becomes part of the listener forever.")
        .Cast(p =>
          {
            p.TimingRule(new FirstMain());
            p.Effect = () => new PutIntoPlay {Category = EffectCategories.ToughnessIncrease};
          })
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new AddPowerAndToughness(1, 1);
            p.CardFilter = (card, effect) => card.Controller == effect.Source.Controller && card.Is().Creature;
          });
    }
  }
}