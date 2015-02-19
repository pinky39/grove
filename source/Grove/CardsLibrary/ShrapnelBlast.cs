namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class ShrapnelBlast : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Shrapnel Blast")
        .ManaCost("{1}{R}")
        .Type("Instant")
        .Text(
          "As an additional cost to cast Shrapnel Blast, sacrifice an artifact.{EOL}Shrapnel Blast deals 5 damage to target creature or player.")
        .FlavorText("It requires immense pressure to imbue an item with magic. If the spell is broken...")
        .Cast(p =>
          {
            p.Cost = new AggregateCost(
              new PayMana("{1}{R}".Parse()),
              new Sacrifice());

            p.TargetSelector.AddCost(trg =>
              {
                trg.Is.Card(c => c.Is().Artifact, ControlledBy.SpellOwner).On.Battlefield();
                trg.Message = "Select an artifact to sacrifice.";
              });

            p.Text = "Shrapnel Blast deals 5 damage to target creature or player.";
            p.Effect = () => new DealDamageToTargets(5);
            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
            
            p.TargetingRule(new CostSacrificeEffectDealDamage(5));
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));
          });
    }
  }
}