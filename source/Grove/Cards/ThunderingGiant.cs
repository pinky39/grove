namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Dsl;

  public class ThunderingGiant : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Thundering Giant")
        .ManaCost("{3}{R}{R}")
        .Type("Creature Giant")
        .Text("{Haste}")
        .FlavorText("The giant was felt a few seconds before he was seen.")
        .Power(4)
        .Toughness(3)
        .Timing(Timings.FirstMain())
        .Abilities(
          Static.Haste
        );
    }
  }
}