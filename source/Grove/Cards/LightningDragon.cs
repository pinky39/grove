﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.RepetitionRules;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Abilities;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class LightningDragon : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Lightning Dragon")
        .ManaCost("{2}{R}{R}")
        .Type("Creature - Dragon")
        .Text(
          "{Flying};{echo} (At the beginning of your upkeep, if this came under your control since the beginning of your last upkeep, sacrifice it unless you pay its echo cost.){EOL}{R}: Lightning Dragon gets +1/+0 until end of turn.")
        .Power(4)
        .Toughness(4)
        .Echo("{2}{R}{R}")
        .SimpleAbilities(Static.Flying)
        .ActivatedAbility(p =>
          {
            p.Text = "{R}: Lightning Dragon gets +1/+0 until end of turn.";
            p.Cost = new PayMana(Mana.Red, ManaUsage.Abilities, supportsRepetitions: true);
            p.Effect = () => new ApplyModifiersToSelf(() => new AddPowerAndToughness(1, 0) {UntilEot = true});
            p.TimingRule(new IncreaseOwnersPowerOrToughness(1, 0));
            p.RepetitionRule(new MaxRepetitions());
          });
    }
  }
}