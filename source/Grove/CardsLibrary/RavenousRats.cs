namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Triggers;

  public class RavenousRats : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Ravenous Rats")
        .ManaCost("{1}{B}")
        .Type("Creature Rat")
        .Text("When Ravenous Rats enters the battlefield, target opponent discards a card.")
        .FlavorText("For all the priceless tomes they have destroyed, one would think they would taste better.")
        .OverrideScore(p => p.Battlefield = Scores.ManaCostToScore[1])
        .Power(1)
        .Toughness(1)
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "When Ravenous Rats enters the battlefield, target opponent discards a card.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new OpponentDiscardsCards(selectedCount: 1);
          });
    }
  }
}