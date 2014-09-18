namespace Grove.CardsLibrary
{
    using System.Collections.Generic;

    public class WitchsFamiliar : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Witch's Familiar")
                .ManaCost("{2}{B}")
                .Type("Creature - Frog")
                .FlavorText("Some bog witches practice the strange art of batrachomancy, reading portents in the number, size, and color of warts on a toad's hide.")
                .Power(2)
                .Toughness(3);
        }
    }
}
