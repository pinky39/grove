namespace Grove.CardsLibrary
{
    using System.Collections.Generic;

    public class WillForgedGolem : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Will-Forged Golem")
                .ManaCost("{6}")
                .Type("Artifact Creature — Golem")
                .Text("{Convoke}{I}(Your creatures can help cast this spell. Each creature you tap while casting this spell pays for {1} or one mana of that creature's color.){/I}")
                .FlavorText("The modular nature of the automaton's design makes assembly perfectly intuitive.")
                .Power(4)
                .Toughness(4)
                .Convoke();
        }
    }
}
