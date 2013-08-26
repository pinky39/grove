namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class Knighthood : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Knighthood")
        .ManaCost("{2}{W}")
        .Type("Enchantment")
        .Text("Creatures you control have first strike.")
        .FlavorText("He has returned. He who brought the dark ones. He who poisoned our paradise. How shall we greet him? With swift and certain death.")
        .Cast(p =>
          {
            p.TimingRule(new OnFirstMain());
            p.TimingRule(new WhenYouDontControlSamePermanent());
          })
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new AddStaticAbility(Static.FirstStrike);
            p.CardFilter = (card, effect) => card.Controller == effect.Source.Controller && card.Is().Creature;
          });
    }
  }
}