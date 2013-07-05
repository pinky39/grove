namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Misc;
  using Gameplay.States;

  public class SimianGrunts : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Simian Grunts")
        .ManaCost("{2}{G}")
        .Type("Creature Ape")
        .Text(
          "{Flash}{EOL}{Echo} {2}{G}(At the beginning of your upkeep, if this came under your control since the beginning of your last upkeep, sacrifice it unless you pay its echo cost.)")
        .FlavorText("These monkeys mean business.")
        .Power(3)
        .Toughness(4)
        .SimpleAbilities(Static.Flash)
        .Cast(p => p.TimingRule(new Steps(activeTurn: false, passiveTurn: true, steps: Step.DeclareAttackers)))
        .Echo("{2}{G}");
    }
  }
}