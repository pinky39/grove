namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Modifiers;
  using Core.Preventions;

  public class Lull : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Lull")
        .ManaCost("{1}{G}")
        .Type("Instant")
        .Text(
          "Prevent all combat damage that would be dealt this turn.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Timing = All(Timings.Steps(Step.DeclareBlockers), Timings.Turn(active: false));
            p.Effect = Effect<Core.Effects.ApplyModifiersToPlayer>(e =>
              {
                e.Player = e.Players.Passive;
                e.Modifiers(Modifier<AddContiniousEffect>(m =>
                  {
                    m.AddLifetime(Lifetime<EndOfTurnLifetime>());

                    m.Effect =
                      Continuous(c =>
                        {
                          c.ModifierFactory = Modifier<AddDamagePrevention>(
                            m1 =>
                              m1.Prevention = Prevention<PreventDealtCombat>());
                          c.CardFilter = (card, self) => card.Is().Creature;
                        });
                  }));
              });
          });
    }
  }
}