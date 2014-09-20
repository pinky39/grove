namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using Effects;
  using Triggers;

  public class GreatWhale : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Great Whale")
        .ManaCost("{5}{U}{U}")
        .Type("Creature Whale")
        .Text("When Great Whale enters the battlefield, untap up to seven lands.")
        .FlavorText("As a great whale dies, it flips onto its back. And so an island is born.")
        .OverrideScore(p => p.Battlefield = Scores.ManaCostToScore[5])
        .Power(5)
        .Toughness(5)
        .TriggeredAbility(p =>
          {
            p.Text = "When Great Whale enters the battlefield, untap up to seven lands.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new UntapSelectedPermanents(
              minCount: 0,
              maxCount: 7,
              validator: c => c.Is().Land,
              text: "Select lands to untap."
              );
          }
        );
    }
  }
}