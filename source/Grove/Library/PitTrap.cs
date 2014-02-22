namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;

  public class PitTrap : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Pit Trap")
        .ManaCost("{2}")
        .Type("Artifact")
        .Text("{2},{T}, Sacrifice Pit Trap: Destroy target attacking creature without flying. It can't be regenerated.")
        .FlavorText("Yotian soldiers were designed to fight, not watch their feet.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .ActivatedAbility(p =>
          {
            p.Text =
              "{2},{T}, Sacrifice Pit Trap: Destroy target attacking creature without flying. It can't be regenerated.";

            p.Cost = new AggregateCost(
              new PayMana(2.Colorless(), ManaUsage.Abilities),
              new Tap(),
              new Sacrifice());

            p.Effect = () => new DestroyTargetPermanents(canRegenerate: false);

            p.TargetSelector.AddEffect(trg => trg.Is
              .Card(c => c.Is().Creature && c.IsAttacker && !c.Has().Flying)
              .On.Battlefield());

            p.TimingRule(new AfterOpponentDeclaresAttackers());
            p.TargetingRule(new EffectDestroy());            
          });
    }
  }
}