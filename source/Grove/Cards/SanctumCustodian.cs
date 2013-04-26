﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Damage;
  using Gameplay.Effects;
  using Gameplay.Modifiers;

  public class SanctumCustodian : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Sanctum Custodian")
        .ManaCost("{2}{W}")
        .Type("Creature Human Cleric")
        .Text("{T}: Prevent the next 2 damage that would be dealt to target creature or player this turn.")
        .FlavorText("Serra told them to guard Urza as he healed. Five years they stood.")
        .Power(1)
        .Toughness(2)
        .ActivatedAbility(p =>
          {
            p.Text = "{T}: Prevent the next 2 damage that would be dealt to target creature or player this turn.";
            p.Cost = new Tap();
            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddDamagePrevention(new PreventDamage(2)) {UntilEot = true});

            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
            p.TargetingRule(new PreventDamageToTargets(2));
          }
        );
    }
  }
}