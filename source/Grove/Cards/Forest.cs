namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.Misc;

  public class Forest : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Forest")
        .Type("Basic Land - Forest")
        .Text("{T}: Add {G} to your mana pool.")
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {G} to your mana pool.";
            p.ManaAmount(Mana.Green);
          });
    }
  }
}