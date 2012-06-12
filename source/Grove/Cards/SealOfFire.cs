namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Costs;
  using Core.Effects;

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
        .Abilities(
          C.ActivatedAbility(
            "Sacrifice Seal of Fire: Seal of Fire deals 2 damage to target creature or player.",
            C.Cost<SacrificeOwner>(),
            C.Effect<DealDamageToTarget>((e, _) => e.Amount = 2),
            selector: C.Selector(
              validator: target => target.IsPlayer() || target.Is().Creature,
              scorer: Core.Ai.TargetScores.OpponentStuffScoresMore(spellsDamage: 2)),
            timing: Any(Timings.InstantRemoval)));     
    }
  }
}