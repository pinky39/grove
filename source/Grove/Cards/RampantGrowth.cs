namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Effects;

  public class RampantGrowth : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Rampant Growth")
        .ManaCost("{1}{G}")
        .Type("Sorcery")
        .Text(
          "Search your library for a basic land card and put that card onto the battlefield tapped. Then shuffle your library.")
        .FlavorText("I've never heard growth before.")
        .Cast(p =>
          {
            p.Effect = () => new SearchLibraryPutToZone(
              c => { c.PutToBattlefield(); c.Tap(); },
              minCount: 0,
              maxCount: 1,
              validator: (e, c) => c.Is().BasicLand,
              text: "Search your library for a basic land card.");
          });
    }
  }
}