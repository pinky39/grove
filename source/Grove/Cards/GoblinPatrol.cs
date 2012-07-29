namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;

  public class GoblinPatrol : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Goblin Patrol")
        .ManaCost("{R}")
        .Type("Creature Goblin")
        .Text(
          "{Echo} {R} (At the beginning of your upkeep, if this came under your control since the beginning of your last upkeep, sacrifice it unless you pay its echo cost.)")
        .FlavorText("'Take the sharp metal stick and make a lotta holes.'{EOL}—Jula, goblin raider")
        .Power(2)
        .Toughness(1)
        .Timing(Timings.Creatures())
        .Echo("{R}");
    }
  }
}