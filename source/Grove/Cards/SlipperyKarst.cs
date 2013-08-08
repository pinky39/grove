namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class SlipperyKarst : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Slippery Karst")
        .Type("Land")
        .Text(
          "Slippery Karst enters the battlefield tapped.{EOL}{T}: Add {G} to your mana pool.{EOL}{Cycling} {2}({2}, Discard this card: Draw a card.)")
        .Cast(p => p.Effect = () => new PutIntoPlay(tap: true))
        .Cycling("{2}")
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {G} to your mana pool.";
            p.ManaAmount(Mana.Green);
          });
    }
  }
}