namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Zones;

  public class RampantGrowth : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
              zone: Zone.Battlefield,
              afterPutToZone: c => c.Tap(),
              minCount: 0,
              maxCount: 1,
              validator: (e, c) => c.Is().BasicLand,
              text: "Search your library for a basic land card.");
          });
    }
  }
}