namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Costs;
  using Effects;

  public class RecklessAbandon : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Reckless Abandon")
        .ManaCost("{R}")
        .Type("Sorcery")
        .Text(
          "As an additional cost to cast Reckless Abandon, sacrifice a creature.{EOL}Reckless Abandon deals 4 damage to target creature or player.")
        .FlavorText("The climax of a warlord's career is always death.")
        .Cast(p =>
          {
            p.Cost = new AggregateCost(
              new PayMana(Mana.Red, ManaUsage.Spells),
              new Sacrifice());

            p.Effect = () => new DealDamageToTargets(4);
            p.TargetSelector
              .AddCost(trg => trg.Is.Creature(ControlledBy.SpellOwner).On.Battlefield())
              .AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());

            p.TargetingRule(new CostSacrificeEffectDealDamage(4));
          });
    }
  }
}