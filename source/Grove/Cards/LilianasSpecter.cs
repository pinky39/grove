namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Dsl;
  using Core.Zones;

  public class LilianasSpecter : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Liliana's Specter")
        .ManaCost("{1}{B}{B}")
        .Type("Creature - Specter")
        .Text("{Flying}{EOL}When Liliana's Specter enters the battlefield, each opponent discards a card.")
        .FlavorText("'The finest minions know what I need without me ever saying a thing.'")
        .Power(2)
        .Toughness(1)
        .Timing(Timings.FirstMain())
        .Abilities(
          Static.Flying,
          TriggeredAbility(
            "When Liliana's Specter enters the battlefield, each opponent discards a card.",
            Trigger<OnZoneChange>(t => t.To = Zone.Battlefield),
            Effect<OpponentDiscardsCards>(e => e.SelectedCount = 1)));
    }
  }
}