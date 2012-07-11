namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class SealOfFire : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Seal of Fire")
        .ManaCost("{R}")
        .Type("Enchantment")
        .Text("Sacrifice Seal of Fire: Seal of Fire deals 2 damage to target creature or player.")
        .FlavorText("'I am the romancer, the passion that consumes the flesh.'{EOL}—Seal inscription")
        .Timing(Timings.NoRestrictions())
        .Abilities(
          C.ActivatedAbility(
            "Sacrifice Seal of Fire: Seal of Fire deals 2 damage to target creature or player.",
            C.Cost<SacrificeOwner>(),
            C.Effect<DealDamageToTarget>((e, _) => e.SetAmount(2)),
            effectSelector: C.Selector(Selectors.CreatureOrPlayer()),
            targetFilter: TargetFilters.DealDamage(2),
            timing: Timings.TargetRemovalInstant()));
    }
  }
}