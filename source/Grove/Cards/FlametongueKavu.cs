namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TargetingRules;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Triggers;
  using Core.Zones;

  public class FlametongueKavu : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Flametongue Kavu")
        .ManaCost("{3}{R}")
        .Type("Creature Kavu")
        .Text("When Flametongue Kavu enters the battlefield, it deals 4 damage to target creature.")
        .FlavorText("For dim-witted, thick-skulled genetic mutants, they have pretty good aim.")
        .Power(4)
        .Toughness(2)
        .Cast(p => p.TimingRule(new OpponentHasPermanents(
          card => card.Is().Creature && card.Life <= 4 &&
            card.CanBeTargetBySpellsWithColor(CardColor.Red))))
        .TriggeredAbility(p =>
          {
            p.Text = "When Flametongue Kavu enters the battlefield, it deals 4 damage to target creature.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new DealDamageToTargets(4);
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new DealDamage(4));
          });
    }
  }
}