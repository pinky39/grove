namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;

  public class BlanchwoodTreefolk : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Blanchwood Treefolk")
        .ManaCost("{4}{G}")
        .Type("Creature - Treefolk")
        .FlavorText(
          "The massive Argivian attack on their rooted kindred was a declaration of war to the treefolk.")
        .Power(4)
        .Toughness(5)
        .Timing(Timings.Creatures());
    }
  }
}