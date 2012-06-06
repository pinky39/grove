namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Effects;

  public class BurstLightning : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
      .Named("Burst Lightning")
      .ManaCost("{R}")
      .KickerCost("{4}")
      .Type("Instant")
      .Text(
        "{Kicker} {4}{EOL}Burst Lightning deals 2 damage to target creature or player. If Burst Lightning was kicked, it deals 4 damage to that creature or player instead.")
      .Effect<DealDamageToTarget>((e, _) => e.Amount = 2)
      .Timing(Timings.InstantRemoval)
      .Category(EffectCategories.DamageDealing)
      .Target(C.Selector(
        validator: target => target.IsPlayer() || target.Is().Creature,
        scorer: Core.Ai.TargetScores.OpponentStuffScoresMore(2)))
      .KickerEffect<DealDamageToTarget>((e, _) => e.Amount = 4)
      .KickerTarget(C.Selector(
        validator: target => target.IsPlayer() || target.Is().Creature,
        scorer: Core.Ai.TargetScores.OpponentStuffScoresMore(4)));
    }
  }  
}