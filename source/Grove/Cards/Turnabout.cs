namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.States;

  public class Turnabout : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Turnabout")
        .ManaCost("{2}{U}{U}")
        .Type("Instant")
        .Text(
          "Choose artifact, creature, or land. Tap all untapped permanents of the chosen type target player controls, or untap all tapped permanents of that type that player controls.")
        .FlavorText("The best cure for a big ego is a little failure.")
        .Cast(p =>
          {
            p.Effect = () => new TapOrUntapAllArtifactsCreaturesOrLands();
            p.TargetSelector.AddEffect(trg => trg.Is.Player());

            p.TimingRule(new Any(
              new Steps(activeTurn: false, passiveTurn: true, steps: Step.DeclareAttackers),
              new Steps(activeTurn: true, passiveTurn: false, steps: Step.BeginningOfCombat),
              new Steps(activeTurn: false, passiveTurn: true, steps: Step.Upkeep)));

            p.TargetingRule(new SelectPlayer((tp, g) =>
              {
                if (g.Turn.Step == Step.DeclareAttackers)
                  return tp.Controller;

                return tp.Controller.Opponent;
              }));
          });
    }
  }
}