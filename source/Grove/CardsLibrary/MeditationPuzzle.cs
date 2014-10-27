namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using AI.TimingRules;
    using Effects;

    public class MeditationPuzzle : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Meditation Puzzle")
                .ManaCost("{3}{W}{W}")
                .Type("Instant")
                .Text("{Convoke}{I}(Your creatures can help cast this spell. Each creature you tap while casting this spell pays for {1} or one mana of that creature's color.){/I}{EOL}You gain 8 life.")
                .FlavorText("Find your center, and you will find your way.")
                .Convoke()
                .Cast(p =>
                {
                    p.Text = "You gain 8 life.";
                    p.Effect = () => new ChangeLife(amount: 8, yours: true);
                    p.TimingRule(new Any(new AfterOpponentDeclaresAttackers(), new WhenYourLifeCanBecomeZero()));
                });
        }
    }
}
