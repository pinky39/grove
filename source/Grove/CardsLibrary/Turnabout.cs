namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;

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
              new OnOpponentsTurn(Step.DeclareAttackers),
              new OnYourTurn(Step.BeginningOfCombat),
              new OnOpponentsTurn(Step.Upkeep)));

            p.TargetingRule(new EffectSelectPlayer((tp, g) =>
              {
                if (g.Turn.Step == Step.DeclareAttackers)
                  return tp.Controller;

                return tp.Controller.Opponent;
              }));
          });
    }
  }
}