namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Ai;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Modifiers;

  public class GloriousAnthem : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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