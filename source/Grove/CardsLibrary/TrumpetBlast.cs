namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class TrumpetBlast : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Trumpet Blast")
        .ManaCost("{2}{R}")
        .Type("Instant")
        .Text("Attacking creatures get +2/+0 until end of turn.")
        .FlavorText("Keldon warriors don't need signals to tell them when to attack, but when to stop.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToPermanents(
              selector: (e, c) => c.IsAttacker,
              modifiers: () => new AddPowerAndToughness(2, 0) {UntilEot = true});

            p.TimingRule(new OnYourTurn(Step.DeclareBlockers));
          });
    }
  }
}