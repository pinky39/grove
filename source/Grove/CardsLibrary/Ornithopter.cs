namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using AI.TimingRules;

    public class Ornithopter : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Ornithopter")
                .ManaCost("{0}")
                .Type("Artifact Creature — Thopter")
                .Text("{Flying}{I}(This creature can't be blocked except by creatures with flying or reach.){/I}")
                .FlavorText("Once a year, the skies over Paliano fill with the flying machines of those who hope to be taken on as pupils by the artificer Muzzio.")
                .Power(0)
                .Toughness(2)
                .Cast(p => p.TimingRule(new OnFirstMain()))
                .SimpleAbilities(Static.Flying);
        }
    }
}
