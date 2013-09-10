namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.States;

  public class FranticSearch : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Frantic Search")
        .ManaCost("{2}{U}")
        .Type("Instant")
        .Text("Draw two cards, then discard two cards. Untap up to three lands.")
        .FlavorText("Motivation was high in the academy once students realized flunking their exams could kill them.")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new DrawCards(count: 2, discardCount: 2),
              new UntapSelectedPermanents(
                minCount: 0,
                maxCount: 3,
                validator: c => c.Is().Land,
                text: "Select lands to untap."
                )
              );

            p.TimingRule(new OnYourTurn(Step.FirstMain));
          });
    }
  }
}