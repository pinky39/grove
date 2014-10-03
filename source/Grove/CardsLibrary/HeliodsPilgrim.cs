namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using Effects;
    using Triggers;

    public class HeliodsPilgrim : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Heliod's Pilgrim")
                .ManaCost("{2}{W}")
                .Type("Creature — Human Cleric")
                .Text("When Heliod's Pilgrim enters the battlefield, you may search your library for an Aura card, reveal it, put it into your hand, then shuffle your library.")
                .FlavorText("The blessings of Heliod are apparent for all to see.")
                .Power(1)
                .Toughness(2)
                .TriggeredAbility(p =>
                {
                    p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

                    p.Effect = () => new SearchLibraryPutToZone(
                          zone: Zone.Hand,
                          minCount: 0,
                          maxCount: 1,
                          validator: (e, c) => c.Is().Aura,
                          text: "Search your library for an Aura card.");
                });
        }
    }
}
