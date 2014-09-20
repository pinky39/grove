namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using AI.TimingRules;

    public class IllusoryAngel : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Illusory Angel")
                .ManaCost("{2}{U}")
                .Type("Creature - Angel Illusion")
                .Text("{Flying}{EOL}Cast Illusory Angel only if you've cast another spell this turn.")
                .FlavorText(
                    "\"In a daze, I woke and looked upon the battlefield, where I could swear my dreams were laying waste to the enemy.\"—Letter from a soldier")
                .Power(4)
                .Toughness(4)
                .SimpleAbilities(Static.Flying)
                .Cast(p =>
                {
                    p.Condition = (player, game) => game.Turn.Events.HasActivePlayerPlayedAnySpell;

                    p.TimingRule(new OnSecondMain());
                });
        }
    }
}
