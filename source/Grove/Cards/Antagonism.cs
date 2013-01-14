namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Effects;
  using Core.Triggers;

  public class Antagonism : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Antagonism")
        .ManaCost("{3}{R}")
        .Type("Enchantment")        
        .Text(
          "At the beginning of each player's end step, Antagonism deals 2 damage to that player unless one of his or her opponents was dealt damage this turn.")
        .Cast(p => p.Timing = Timings.SecondMain())
        .Abilities(
          TriggeredAbility(
            "At the beginning of each player's end step, Antagonism deals 2 damage to that player unless one of his or her opponents was dealt damage this turn.",
            Trigger<OnTurnOpponentWasNotDealtDamage>(),
            Effect<DealDamageToActivePlayer>(p => p.Amount = 2), triggerOnlyIfOwningCardIsInPlay: true)
        );
    }
  }
}