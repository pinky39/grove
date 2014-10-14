namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class Grindclock : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Grindclock")
        .ManaCost("{2}")
        .Type("Artifact")
        .Text("{T}: Put a charge counter on Grindclock.{EOL}{T}: Target player puts the top X cards of his or her library into his or her graveyard, where X is the number of charge counters on Grindclock.")
        .FlavorText("Pray you never hear it chime.")
        .ActivatedAbility(p =>
        {
          p.Text = "{T}: Put a charge counter on Grindclock.";
          p.Cost = new Tap();
          p.Effect = () => new ApplyModifiersToSelf(
            () => new AddCounters(() => new SimpleCounter(CounterType.Charge), 1));
          p.TimingRule(new OnEndOfOpponentsTurn());
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{T}: Target player puts the top X cards of his or her library into his or her graveyard, where X is the number of charge counters on Grindclock.";
          p.Cost = new Tap();
          p.Effect = () => new PlayerPutsTopCardsFromLibraryToGraveyard(
            player: null,
            count: P(e => e.Source.OwningCard.CountersCount(CounterType.Charge)));

          p.TargetSelector.AddEffect(trg => trg.Is.Player());

          p.TargetingRule(new EffectOpponent());
          p.TimingRule(new WhenCardHasCounters(3));
        });
    }
  }
}
