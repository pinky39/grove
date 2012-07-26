namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class ArcLightning : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Arc Lightning")
        .ManaCost("{2}{R}")
        .Type("Sorcery")
        .Timing(Timings.NoRestrictions())
        .Text(
          "Arc Lightning deals 3 damage divided as you choose among one, two, or three target creatures and/or players.")
        .FlavorText("Rainclouds don't last long in Shiv, but that doesn't stop the lightning.")
        .Effect<DealDamageToTargets>(e => e.Amount = 3)
        .DistributeSpellsDamage()
        .Targets(
          aiTargetSelector: AiTargetSelectors.DealDamageDistribute(3),
          effectValidator: C.Validator(Validators.CreatureOrPlayer(), maxCount: 3)
        );
    }
  }
}