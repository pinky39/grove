namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.Misc;

  public class UnworthyDead : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Unworthy Dead")
        .ManaCost("{1}{B}")
        .Type("Creature Skeleton")
        .Text("{B}: Regenerate Unworthy Dead.")
        .FlavorText(
          "Great Yawgmoth moves across the seas of shard and bone and rust. We exalt him in life, in death, and in between.")
        .Power(1)
        .Toughness(1)
        .Regenerate(cost: Mana.Black, text: "{B}: Regenerate Unworthy Dead.");
    }
  }
}