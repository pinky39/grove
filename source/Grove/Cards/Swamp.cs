namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.Costs;
  using Gameplay.Misc;

  public class Swamp : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Swamp")
        .Type("Basic Land - Swamp")
        .Text("{T}: Add {B} to your mana pool.")
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {B} to your mana pool.";
            p.ManaAmount(Mana.Black);
          });
    }
  }
}