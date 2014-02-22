namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;

  public class DragonBlood : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Dragon Blood")
        .ManaCost("{3}")
        .Type("Artifact")
        .Text("{3},{T} : Put a +1/+1 counter on target creature.")
        .FlavorText("Fire in the blood, fire in the belly.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .ActivatedAbility(p =>
          {
            p.Text = "{3},{T} : Put a +1/+1 counter on target creature.";
            p.Cost = new AggregateCost(
              new PayMana(3.Colorless(), ManaUsage.Abilities),
              new Tap());

            p.Effect = () => new ApplyModifiersToTargets(() => new AddCounters(
              () => new PowerToughness(1, 1), count: 1));

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new EffectPumpInstant(1, 1, untilEot: false));
          });
    }
  }
}