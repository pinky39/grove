namespace Grove.CardsLibrary
{
    using System.Collections.Generic;

    public class GeistOfTheMoors : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Geist of the Moors")
                .ManaCost("{1}{W}{W}")
                .Type("Creature - Spirit")
                .Text("{Flying}")
                .FlavorText("\"The battle is won. There's work to be done. / The Blessed Sleep must wait. / A fiend is about. It stalks the devout. / I'll save them from my fate.\"{EOL}-The Good Geist's Vow")
                .Power(3)
                .Toughness(1)
                .SimpleAbilities(Static.Flying);
        }
    }
}
