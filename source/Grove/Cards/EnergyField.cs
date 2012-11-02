namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Cards.Preventions;
  using Core.Details.Cards.Triggers;
  using Core.Dsl;
  using Core.Zones;

  public class EnergyField : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Energy Field")
        .ManaCost("{1}{U}")
        .Type("Enchantment")
        .Text(
          "Prevent all damage that would be dealt to you by sources you don't control.{EOL}When a card is put into your graveyard from anywhere, sacrifice Energy Field.")
        .Timing(Timings.FirstMain())
        .Abilities(
          C.Continuous((e, c) =>
            {
              e.ModifierFactory = c.Modifier<AddDamagePrevention>(
                (m, c0) => m.Prevention = c0.Prevention<PreventDamageToTarget>((p, _) =>
                  {
                    p.PreventAll = true;
                    p.SourceFilter = (self, source) => self.Controller != source.Controller;
                  }));
              e.CardFilter = delegate { return false; };
              e.PlayerFilter = (player, field) => player == field.Controller;
            }),
          C.TriggeredAbility(
            "When a card is put into your graveyard from anywhere, sacrifice Energy Field.",
            C.Trigger<OnZoneChange>((t, _) =>
              {
                t.Filter = (ability, card) => ability.Controller == card.Owner;
                t.To = Zone.Graveyard;
              }),
            C.Effect<SacrificeSource>(),
            triggerOnlyIfOwningCardIsInPlay: true
            )
        );
    }
  }
}