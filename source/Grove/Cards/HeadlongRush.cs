namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Modifiers;

  public class HeadlongRush : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Headlong Rush")
        .ManaCost("{1}{R}")
        .Type("Instant")
        .Text("Attacking creatures gain first strike until end of turn.")
        .FlavorText(
          "A landslide of goblins poured towards the defenders—tumbling, rolling, and bouncing their way down the steep hillside.")
        .Cast(p =>
          {
            p.Timing = All(Timings.DeclareBlockers(), Timings.Turn(active: true), Timings.HasAttackers(1));
            p.Effect = Effect<Core.Effects.ApplyModifiersToPermanents>(e =>
              {
                e.Filter = (self, card) => card.IsAttacker;
                e.Modifiers(
                  Modifier<AddStaticAbility>(m => m.StaticAbility = Static.FirstStrike, untilEndOfTurn: true)
                  );
              });
          });
    }
  }
}