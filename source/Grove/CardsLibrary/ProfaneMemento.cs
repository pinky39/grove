namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using Effects;
    using Triggers;

    public class ProfaneMemento : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Profane Memento")
                .ManaCost("{1}")
                .Type("Artifact")
                .Text("Whenever a creature card is put into an opponent's graveyard from anywhere, you gain 1 life.")
                .FlavorText("\"An angel's skull is left too plain by death. I made a few aesthetic modifications.\"—Dommique, blood artist")
                .TriggeredAbility(p =>
                {
                    p.Text = "Whenever a creature card is put into an opponent's graveyard from anywhere, you gain 1 life.";

                    p.Trigger(new OnZoneChanged(
                      to: Zone.Graveyard,
                      filter: (c, a, _) => c.Owner != a.OwningCard.Controller && c.Is().Creature));

                    p.Effect = () => new ChangeLife(amount: 1, forYou: true);

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
