namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.CardDsl;
  using Core.Effects;
  using Core.Triggers;
  using Core.Zones;

  public class LilianasSpecter : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Liliana's Specter")
        .ManaCost("{1}{B}{B}")
        .Type("Creature - Specter")
        .Text("{Flying}{EOL}When Liliana's Specter enters the battlefield, each opponent discards a card.")
        .FlavorText("'The finest minions know what I need without me ever saying a thing.'")
        .Power(2)
        .Toughness(1)
        .Abilities(
          StaticAbility.Flying,
          C.TriggeredAbility(
            "When Liliana's Specter enters the battlefield, each opponent discards a card.",
            C.Trigger<ChangeZone>((t, _) => t.To = Zone.Battlefield),
            C.Effect<OpponentDiscardsCards>((e, _) => e.SelectedCount = 1)));
    }
  }
}