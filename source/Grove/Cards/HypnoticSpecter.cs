namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Dsl;

  public class HypnoticSpecter : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Hypnotic Specter")
        .ManaCost("{1}{B}{B}")
        .Type("Creature - Specter")
        .Text(
          "{Flying}{EOL}Whenever Hypnotic Specter deals damage to an opponent, that player discards a card at random.")
        .FlavorText("'Its victims are known by their eyes shattered vessels leaking broken dreams.")
        .Power(2)
        .Toughness(2)
        .Timing(Timings.Creatures())
        .Abilities(
          Static.Flying,
          C.TriggeredAbility(
            "Whenever Hypnotic Specter deals damage to an opponent, that player discards a card at random.",
            C.Trigger<DealDamageToCreatureOrPlayer>((t, c) => t.ToOpponent()),
            C.Effect<OpponentDiscardsCards>(e => e.RandomCount = 1)));
    }
  }
}