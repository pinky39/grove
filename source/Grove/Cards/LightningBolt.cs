namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Effects;

  public class LightningBolt : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Lightning Bolt")
        .ManaCost("{R}")
        .Type("Instant")
        .Text("Lightning Bolt deals 3 damage to target creature or player.")
        .Timing(Timings.InstantRemoval)
        .Category(EffectCategories.DamageDealing)
        .Effect<DealDamageToTarget>((e, _) => e.Amount = 3)
        .Target(C.Selector(
          validator: target => target.IsPlayer() || target.Is().Creature,
          scorer: Core.Ai.TargetScores.OpponentStuffScoresMore(spellsDamage: 3)));
    }
  }
}