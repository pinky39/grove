namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;

  public class Shock : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Shock")
        .ManaCost("{R}")
        .Type("Instant")
        .Timing(Timings.InstantRemoval())
        .Text("Shock deals 2 damage to target creature or player.")
        .Effect<DealDamageToTarget>((e, _) => e.Amount = 2)        
        .Targets(C.Selector(
          validator: target => target.IsPlayer() || target.Is().Creature,
          scorer: Core.Ai.TargetScores.OpponentStuffScoresMore(spellsDamage: 2)));
    }
  }
}