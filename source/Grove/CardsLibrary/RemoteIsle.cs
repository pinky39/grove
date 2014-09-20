namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;

  public class RemoteIsle : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Remote Isle")
        .Type("Land")
        .Text(
          "Remote Isle enters the battlefield tapped.{EOL}{T}: Add {U} to your mana pool.{EOL}{Cycling} {2}({2}, Discard this card: Draw a card.)")
        .Cast(p => p.Effect = () => new PutIntoPlay(tap: true))
        .Cycling("{2}")
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {U} to your mana pool.";
            p.ManaAmount(Mana.Blue);
          });
    }
  }
}