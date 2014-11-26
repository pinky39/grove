namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Modifiers;
  using Triggers;

  public class RakshasaVizier : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Rakshasa Vizier")
        .ManaCost("{2}{B}{G}{U}")
        .Type("Creature - Cat Demon")
        .Text("Whenever one or more cards are put into exile from your graveyard, put that many +1/+1 counters on Rakshasa Vizier.")
        .FlavorText("Rakshasa offer deals that sound advantageous to those who forget who they are dealing with.")
        .Power(4)
        .Toughness(4)
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever one or more cards are put into exile from your graveyard, put that many +1/+1 counters on Rakshasa Vizier.";

          p.Trigger(new OnZoneChanged(
            from: Zone.Graveyard,
            to: Zone.Exile,
            filter: (c, a, _) => c.Controller == a.OwningCard.Controller && c != a.OwningCard));

          p.Effect = () => new ApplyModifiersToSelf(
            () => new AddCounters(() => new PowerToughness(1, 1), count: 1));

          p.TriggerOnlyIfOwningCardIsInPlay = true;
        });
    }
  }
}
