namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Dsl;

  public class Falter : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Falter")
        .ManaCost("{1}{R}")
        .Type("Instant")
        .Text("Creatures without flying can't block this turn.")
        .FlavorText("Like a sleeping dragon, Shiv stirs and groans at times.")
        .Timing(All(Timings.Steps(Step.BeginningOfCombat), Timings.Turn(active: true)))
        .Effect<ApplyModifiersToPlayer>(e =>
          {
            e.Player = e.Players.Passive;
            e.Modifiers(Modifier<AddContiniousEffect>(m =>
              {
                m.AddLifetime(Lifetime<EndOfTurnLifetime>());

                m.Effect =
                  Continuous(c =>
                    {
                      c.ModifierFactory = Modifier<AddStaticAbility>(
                        m1 => m1.StaticAbility = Static.CannotBlock);

                      c.CardFilter = (card, effect) =>
                        (card.Is().Creature && !card.Has().Flying);
                    });
              }));
          });
    }
  }
}