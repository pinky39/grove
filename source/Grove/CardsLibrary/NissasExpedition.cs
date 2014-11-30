namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using Effects;

    public class NissasExpedition : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Nissa's Expedition")
                .ManaCost("{4}{G}")
                .Type("Sorcery")
                .Text("{Convoke}{I}(Your creatures can help cast this spell. Each creature you tap while casting this spell pays for {1} or one mana of that creature's color.){/I}{EOL}Search your library for up to two basic land cards, put them onto the battlefield tapped, then shuffle your library.")
                .SimpleAbilities(Static.Convoke)
                .Cast(p =>
                {
                    p.Text = "Search your library for up to two basic land cards, put them onto the battlefield tapped, then shuffle your library.";
                    p.Effect = () => new SearchLibraryPutToZone(
                        zone: Zone.Battlefield,
                        afterPutToZone: (c, g) => c.Tap(),
                        minCount: 0,
                        maxCount: 2,
                        validator: (e, c) => c.Is().BasicLand,
                        text: "Search your library for up to two basic land cards.");
                });
        }
    }
}
