namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;

  public class ExpendableTroops : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Expendable Troops")
        .ManaCost("{1}{W}")
        .Type("Creature Human Soldier")
        .Text(
          "{T}, Sacrifice Expendable Troops: Expendable Troops deals 2 damage to target attacking or blocking creature.")
        .FlavorText("No doubt in their minds, no fear in their hearts.")
        .Power(2)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text =
              "{T}, Sacrifice Expendable Troops: Expendable Troops deals 2 damage to target attacking or blocking creature.";

            p.Cost = new AggregateCost(
              new Tap(),
              new Sacrifice());

            p.Effect = () => new DealDamageToTargets(2);

            p.TargetSelector.AddEffect(trg => trg.Is
              .Card(c => c.Is().Creature && (c.IsAttacker || c.IsBlocker))
              .On.Battlefield());


            p.TimingRule(new OnStep(Step.DeclareBlockers));
            p.TargetingRule(new EffectDealDamage(2));
          });
    }
  }
}