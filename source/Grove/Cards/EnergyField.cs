namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Cards.Preventions;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Zones;

  public class EnergyField : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Energy Field")
        .ManaCost("{1}{U}")
        .Type("Enchantment")
        .Text(
          "Prevent all damage that would be dealt to you by sources you don't control.{EOL}When a card is put into your graveyard from anywhere, sacrifice Energy Field.")
        .Timing(Timings.FirstMain())
        .Abilities(
          Continuous(e =>
            {
              e.ModifierFactory = Modifier<AddDamagePrevention>(
                m => m.Prevention = Prevention<PreventDamageToTarget>(p =>
                  {
                    p.PreventAll = true;
                    p.SourceFilter = (self, source) => self.Controller != source.Controller;
                  }));
              e.CardFilter = delegate { return false; };
              e.PlayerFilter = (player, field) => player == field.Controller;
            }),
          TriggeredAbility(
            "When a card is put into your graveyard from anywhere, sacrifice Energy Field.",
            Trigger<OnZoneChange>(t =>
              {
                t.Filter = (ability, card) => ability.Controller == card.Owner;
                t.To = Zone.Graveyard;
              }),
            Effect<SacrificeSource>(),
            triggerOnlyIfOwningCardIsInPlay: true
            )
        );
    }
  }
}