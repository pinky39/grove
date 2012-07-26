namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class FlameSlash : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Flame Slash")
        .ManaCost("{R}")
        .Type("Sorcery")
        .Text("Flame Slash deals 4 damage to target creature.")
        .FlavorText(
          "After millennia asleep, the Eldrazi had forgotten about Zendikar's fiery temper and dislike of strangers.")
        .Effect<DealDamageToTarget>(e => e.Amount = 4)
        .Timing(Timings.TargetRemovalInstant())
        .Targets(
          aiTargetSelector: AiTargetSelectors.DealDamage(4),
          effectValidator: C.Validator(
            Validators.Creature()));
    }
  }
}