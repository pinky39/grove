namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Triggers;

  public class Waylay : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
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
            p.Timing = Any(Timings.EndOfTurn(), Timings.SummonBlockers());
            p.Effect = Effect<CreateTokens>(e =>
              {
                e.Count = 3;
                e.Tokens(
                  Card
                    .Named("Knight Token")
                    .FlavorText("'You reek of corruption,' spat the knight. 'Why are you even here?'")
                    .Power(2)
                    .Toughness(2)
                    .OverrideScore(20)
                    .Type("Creature - Token - Knight")
                    .Colors(ManaColors.White)
                    .Abilities(
                      TriggeredAbility(
                        "Exile this at the end of turn.",
                        Trigger<OnStepStart>(t =>
                          {
                            t.Step = Step.EndOfTurn;
                            t.PassiveTurn = true;
                            t.ActiveTurn = true;
                          }),
                        Effect<SacrificeSource>(), triggerOnlyIfOwningCardIsInPlay: true)
                    ));
              });
          });
    }
  }
}