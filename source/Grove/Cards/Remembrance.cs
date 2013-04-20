namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Messages;
  using Core.Triggers;
  using Core.Zones;

  public class Remembrance : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Remembrance")
        .ManaCost("{3}{W}")
        .Type("Enchantment")
        .Text(
          "Whenever a nontoken creature you control dies, you may search your library for a card with the same name as that creature, reveal it, and put it into your hand. If you do, shuffle your library.")
        .Cast(p => p.TimingRule(new FirstMain()))
        .TriggeredAbility(p =>
          {
            p.Trigger(new OnZoneChanged(
              from: Zone.Battlefield,
              to: Zone.Graveyard,
              filter: (a, c) => a.OwningCard.Controller == c.Controller && c.Is().Creature && !c.Is().Token));

            p.Effect = () => new SearchLibraryPutToHand(
              minCount: 0,
              maxCount: 1,
              validator: (e, c) => e.TriggerMessage<ZoneChanged>().Card.Name == c.Name
              );
          });
    }
  }
}