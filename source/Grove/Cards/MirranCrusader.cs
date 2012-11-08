namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Mana;
  using Core.Dsl;

  public class MirranCrusader : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Mirran Crusader")
        .ManaCost("{1}{W}{W}")
        .Type("Creature Human Knight")
        .Text("Double strike, protection from black and from green")
        .FlavorText("A symbol of what Mirrodin once was and hope for what it will be again.")
        .Power(2)
        .Toughness(2)
        .Timing(Timings.Creatures())
        .Protections(ManaColors.Black | ManaColors.Green)
        .Abilities(Static.DoubleStrike);
    }
  }
}