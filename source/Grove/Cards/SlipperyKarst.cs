namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;

  public class SlipperyKarst : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
            p.ManaAmount(ManaAmount.Green);
          });
    }
  }
}