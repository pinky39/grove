namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.CardDsl;
  using Core.Effects;
  using Core.Triggers;

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
        .Abilities(
          StaticAbility.Flying,
          C.TriggeredAbility(
            "Whenever Hypnotic Specter deals damage to an opponent, that player discards a card at random.",
            C.Trigger<DealDamageToPlayer>((t, c) => t.ToOpponent()),
            C.Effect<OpponentDiscardsCards>((e, _) => e.RandomCount = 1)));
    }
  }
}