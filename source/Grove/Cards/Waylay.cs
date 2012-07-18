namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Details.Mana;
  using Core.Dsl;

  public class Waylay : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Waylay")
        .ManaCost("{2}{W}")
        .Type("Instant")
        .Text(
          "Put three Knight tokens into play. Treat these tokens as 2/2 white creatures. Exile them at end of turn.")
        .FlavorText("'You reek of corruption,' spat the knight. 'Why are you even here?'")
        .Timing(Any(Timings.EndOfTurn(), Timings.SummonBlockers()))
        .Effect<CreateTokens>(p =>
          {
            p.Effect.Count = 3;
            p.Effect.Tokens(
              p.Builder.Card
                .Named("Knight Token")
                .FlavorText("'You reek of corruption,' spat the knight. 'Why are you even here?'")
                .Power(2)
                .Toughness(2)
                .Type("Creature - Token - Knight")
                .Colors(ManaColors.White)
                .Abilities(
                  p.Builder.TriggeredAbility(
                    "Exile this at the end of turn.",
                    p.Builder.Trigger<AtBegginingOfStep>((t, _) =>
                      {
                        t.Step = Step.EndOfTurn;
                        t.PassiveTurn = true;
                        t.ActiveTurn = true;
                      }),
                    p.Builder.Effect<SacrificeSource>())
                ));
          });
    }
  }
}