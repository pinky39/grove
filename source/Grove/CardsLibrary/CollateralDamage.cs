namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class CollateralDamage : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Collateral Damage")
        .ManaCost("{R}")
        .Type("Instant")
        .Text("As an additional cost to cast Collateral Damage, sacrifice a creature.{EOL}Collateral Damage deals 3 damage to target creature or player.")
        .FlavorText("It is much easier to create fire than to contain it.")
        .Cast(p =>
        {
          p.Cost = new AggregateCost(
            new PayMana("{R}".Parse(), ManaUsage.Spells),
            new Sacrifice());

          p.TargetSelector.AddCost(trg =>
          {
            trg.Is.Card(c => c.Is().Creature, ControlledBy.SpellOwner).On.Battlefield();
            trg.Message = "Select a creature to sacrifice.";
          });

          p.Text = "Collateral Damage deals 3 damage to target creature or player.";
          p.Effect = () => new DealDamageToTargets(3);
          p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());

          p.TargetingRule(new CostSacrificeEffectDealDamage(3));
          p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));
        });
    }
  }
}
