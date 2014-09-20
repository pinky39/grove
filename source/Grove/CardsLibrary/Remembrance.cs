namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Events;
  using Grove.AI.TimingRules;
  using Grove.Triggers;

  public class Remembrance : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Remembrance")
        .ManaCost("{3}{W}")
        .Type("Enchantment")
        .Text(
          "Whenever a nontoken creature you control dies, you may search your library for a card with the same name as that creature, reveal it, and put it into your hand. If you do, shuffle your library.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever a nontoken creature you control dies, you may search your library for a card with the same name as that creature, reveal it, and put it into your hand. If you do, shuffle your library.";

            p.Trigger(new OnZoneChanged(
              @from: Zone.Battlefield,
              to: Zone.Graveyard,
              filter: (c, a, g) => a.OwningCard.Controller == c.Controller && c.Is().Creature && !c.Is().Token));

            p.Effect = () => new SearchLibraryPutToZone(
              zone: Zone.Hand,
              minCount: 0,
              maxCount: 1,
              validator: (e, c) => e.TriggerMessage<ZoneChangedEvent>().Card.Name == c.Name);
          });
    }
  }
}