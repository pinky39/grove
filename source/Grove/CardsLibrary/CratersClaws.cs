namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.CostRules;
  using AI.TargetingRules;
  using Effects;
  using Modifiers;

  public class CratersClaws : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Crater's Claws")
        .ManaCost("{R}").HasXInCost()
        .Type("Sorcery")
        .Text("Crater's Claws deals X damage to target creature or player.{EOL}{I}Ferocious{/I} — Crater's Claws deals X plus 2 damage to that creature or player instead if you control a creature with power 4 or greater.")
        .Cast(p =>
        {
          p.Effect = () => new DealDamageToTargets(Value.PlusX);
          p.Effect = () => new FerociousEffect(
            L(new DealDamageToTargets(Value.PlusX)),
            L(new DealDamageToTargets(P(e => (e.X ?? 0) + 2))),
            instead: true
            );

          p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());

          p.TargetingRule(new EffectDealDamage());
          p.CostRule(new XIsTargetsLifepointsLeft());
        });
    }
  }
}
