namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using Effects;
    using Triggers;

    public class FirstResponse : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("First Response")
                .ManaCost("{3}{W}")
                .Type("Enchantment")
                .Text("At the beginning of each upkeep, if you lost life last turn, put a 1/1 white Soldier creature token onto the battlefield.")
                .FlavorText("\"There's never a good time for a disaster or an attack. That's why we're here.\"{EOL}—Oren, militia captain")
                .TriggeredAbility(p =>
                {
                    p.Text = "At the beginning of each upkeep, if you lost life last turn, put a 1/1 white Soldier creature token onto the battlefield.";

                    p.Trigger(new OnStepStart(activeTurn: true, passiveTurn: true, step: Step.Upkeep)
                    {
                        Condition = (t, g) => g.Turn.PrevTurnEvents.HasLostLifeFor(t.Controller)
                    });

                    p.Effect = () => new CreateTokens(
                      count: 1,
                      token: Card
                        .Named("Soldier")
                        .Power(1)
                        .Toughness(1)
                        .Type("Token Creature - Soldier")
                        .Colors(CardColor.White));

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
