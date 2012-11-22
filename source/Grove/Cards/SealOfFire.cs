namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class SealOfFire : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Seal of Fire")
        .ManaCost("{R}")
        .Type("Enchantment")
        .Text("Sacrifice Seal of Fire: Seal of Fire deals 2 damage to target creature or player.")
        .FlavorText("'I am the romancer, the passion that consumes the flesh.'{EOL}—Seal inscription")
        .Timing(Timings.NoRestrictions())
        .Abilities(
          ActivatedAbility(
            "Sacrifice Seal of Fire: Seal of Fire deals 2 damage to target creature or player.",
            Cost<SacrificeOwner>(),
            Effect<DealDamageToTargets>(e => e.Amount = 2), 
            TargetValidator(TargetIs.CreatureOrPlayer(), ZoneIs.Battlefield()),
            targetSelectorAi: TargetSelectorAi.DealDamageSingleSelector(2),
            timing: Timings.InstantRemovalTarget()));
    }
  }
}