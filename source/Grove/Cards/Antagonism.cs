namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Dsl;

  public class Antagonism : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Antagonism")
        .ManaCost("{3}{R}")
        .Type("Enchantment")
        .Timing(Timings.SecondMain())
        .Text(
          "At the beginning of each player's end step, Antagonism deals 2 damage to that player unless one of his or her opponents was dealt damage this turn.")
        .Abilities(
          C.TriggeredAbility(
            "At the beginning of each player's end step, Antagonism deals 2 damage to that player unless one of his or her opponents was dealt damage this turn.",
            C.Trigger<OpponentWasNotDealtDamageThisTurn>(),
            C.Effect<DealDamageToActivePlayer>((e, _) => e.Amount = 2), triggerOnlyIfOwningCardIsInPlay: true)
        );
    }
  }
}