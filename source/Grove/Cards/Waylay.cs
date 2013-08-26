namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Characteristics;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.States;
  using Gameplay.Triggers;

  public class Waylay : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Waylay")
        .ManaCost("{2}{W}")
        .Type("Instant")
        .Text(
          "Put three Knight tokens into play. Treat these tokens as 2/2 white creatures. Exile them at end of turn.")
        .FlavorText("'You reek of corruption,' spat the knight. 'Why are you even here?'")
        .Cast(p =>
          {
            p.Effect = () => new CreateTokens(
              count: 3,
              token: Card
                .Named("Knight Token")
                .FlavorText("'You reek of corruption,' spat the knight. 'Why are you even here?'")
                .Power(2)
                .Toughness(2)
                .OverrideScore(new ScoreOverride {Battlefield = 20})
                .Type("Creature - Token - Knight")
                .Colors(CardColor.White)
                .TriggeredAbility(tp =>
                  {
                    tp.Text = "Exile this at the end of turn.";
                    tp.Trigger(new OnStepStart(
                      step: Step.EndOfTurn,
                      passiveTurn: true,
                      activeTurn: true));
                    tp.Effect = () => new ExileOwner();
                    tp.TriggerOnlyIfOwningCardIsInPlay = true;
                  })
              );

            p.TimingRule(new WhenStackIsEmpty());

            p.TimingRule(new Any(
              new OnEndOfOpponentsTurn(),
              new AfterOpponentDeclaresAttackers()));
          });
    }
  }
}