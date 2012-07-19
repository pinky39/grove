namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Dsl;

  public class BullHippo : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Bull Hippo")
        .ManaCost("{3}{G}")
        .Type("Creature Hippo")
        .Text("{Islandwalk} (This creature is unblockable as long as defending player controls an Island.)")
        .FlavorText("'How could you not hear it approach? It's a hippo!'{EOL}—Argivian commander")
        .Power(3)
        .Toughness(3)
        .Timing(Timings.Creatures())
        .Abilities(
          Static.Islandwalk
        );
    }
  }
}