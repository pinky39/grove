namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class Magnify : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Magnify")
        .ManaCost("{G}")
        .Type("Instant")
        .Text("All creatures get +1/+1 until end of turn.")
        .FlavorText("The seed torches' pollen gives off light, but the elves find it heightens their senses as well.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToPermanents(
              selector: (c, ctx) => c.Is().Creature,
              modifier: () => new AddPowerAndToughness(1, 1) {UntilEot = true});

            p.TimingRule(new OnYourTurn(Step.BeginningOfCombat));
          });
    }
  }
}