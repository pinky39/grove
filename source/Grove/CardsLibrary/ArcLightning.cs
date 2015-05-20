namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;

  public class ArcLightning : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
            p.Effect = () => new DistributeDamageToTargets();
            p.DistributeAmount = 3;
            p.TargetSelector.AddEffect(
              trg => trg.Is.CreatureOrPlayer().On.Battlefield(),
              trg => { trg.MaxCount = 3; });
            p.TargetingRule(new EffectDealDamage());
          });
    }
  }
}