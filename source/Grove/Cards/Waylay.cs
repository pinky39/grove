namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Triggers;

  public class Waylay : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
              tokens: Card
                .Named("Knight Token")
                .FlavorText("'You reek of corruption,' spat the knight. 'Why are you even here?'")
                .Power(2)
                .Toughness(2)
                .OverrideScore(20)
                .Type("Creature - Token - Knight")
                .Colors(ManaColors.White)
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
            p.TimingRule(new Any(
              new EndOfTurn(),
              new All(
                new Steps(activeTurn: false, passiveTurn: true, steps: Step.DeclareAttackers),
                new MinAttackerCount(1))));
          });
    }
  }
}