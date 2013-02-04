namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Effects;
  using Core.Targeting;

  public class ArcLightning : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Arc Lightning")
        .ManaCost("{2}{R}")
        .Type("Sorcery")
        .Text(
          "Arc Lightning deals 3 damage divided as you choose among one, two, or three target creatures and/or players.")
        .FlavorText("Rainclouds don't last long in Shiv, but that doesn't stop the lightning.")
        .Cast(p =>
          {

            p.Effect = () => new DealDistributedDamageToTargets(3);
            p.DistributeDamage = true;
            p.EffectTargets = L(Target(Validators.CreatureOrPlayer(), Zones.Battlefield(), maxCount: 3));
            p.DistributeDamage = true;
            p.TargetingAi = TargetingAi.DealDamageSingleSelectorDistribute(3);
          });
    }
  }
}