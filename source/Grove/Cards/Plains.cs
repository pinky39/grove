namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.Misc;

  public class Plains : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Plains")
        .Type("Basic Land - Plains")
        .Text("{T}: Add {W} to your mana pool.")
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {W} to your mana pool.";
            p.ManaAmount(Mana.White);
          });
    }
  }
}