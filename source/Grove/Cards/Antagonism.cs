namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Cards.Triggers;
  using Core.Dsl;

  public class Antagonism : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Antagonism")
        .ManaCost("{3}{R}")
        .Type("Enchantment")
        .Timing(Timings.SecondMain())
        .Text(
          "At the beginning of each player's end step, Antagonism deals 2 damage to that player unless one of his or her opponents was dealt damage this turn.")
        .Abilities(
          TriggeredAbility(
            "At the beginning of each player's end step, Antagonism deals 2 damage to that player unless one of his or her opponents was dealt damage this turn.",
            Trigger<OpponentWasNotDealtDamageThisTurn>(),
            Effect<DealDamageToActivePlayer>(p => p.Amount = 2), triggerOnlyIfOwningCardIsInPlay: true)
        );
    }
  }
}