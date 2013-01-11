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
        .Abilities(
          ActivatedAbility(
            "Sacrifice Seal of Fire: Seal of Fire deals 2 damage to target creature or player.",
            Cost<Sacrifice>(),
            Effect<DealDamageToTargets>(e => e.Amount = 2), 
            Target(Validators.CreatureOrPlayer(), Zones.Battlefield()),
            targetingAi: TargetingAi.DealDamageSingleSelector(2),
            timing: Timings.InstantRemovalTarget()));
    }
  }
}