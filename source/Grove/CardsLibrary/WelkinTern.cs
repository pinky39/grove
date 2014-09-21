namespace Grove.CardsLibrary
{
    using System.Collections.Generic;

    public class WelkinTern : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Welkin Tern")
                .ManaCost("{1}{U}")
                .Type("Creature — Bird")
                .Text("{Flying}{I}(This creature can't be blocked except by creatures with flying or reach.){/I}{EOL}Welkin Tern can block only creatures with flying.")
                .FlavorText("Sailors have come to regard them as bad luck, for they falsely bring hope of land.")
                .Power(2)
                .Toughness(1)
                .SimpleAbilities(Static.Flying, Static.CanBlockOnlyCreaturesWithFlying);
        }
    }
}
