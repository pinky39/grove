namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.Misc;

  public class Mountain : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mountain")
        .Type("Basic Land - Mountain")
        .Text("{T}: Add {R} to your mana pool.")
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {R} to your mana pool.";
            p.ManaAmount(Mana.Red);
          });
    }
  }
}