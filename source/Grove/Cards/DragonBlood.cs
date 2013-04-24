namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Counters;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Mana;
  using Gameplay.Modifiers;

  public class DragonBlood : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Dragon Blood")
        .ManaCost("{3}")
        .Type("Artifact")
        .Text("{3},{T} : Put a +1/+1 counter on target creature.")
        .FlavorText("Fire in the blood, fire in the belly.")
        .Cast(p => p.TimingRule(new FirstMain()))
        .ActivatedAbility(p =>
          {
            p.Text = "{3},{T} : Put a +1/+1 counter on target creature.";
            p.Cost = new AggregateCost(
              new PayMana(3.Colorless(), ManaUsage.Abilities),
              new Tap());

            p.Effect = () => new ApplyModifiersToTargets(() => new AddCounters(
              () => new PowerToughness(1, 1), count: 1));

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new IncreasePowerOrToughness(1, 1, untilEot: false));
          });
    }
  }
}