namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;

  public class JacesIngenuity : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Jace's Ingenuity")
        .ManaCost("{3}{U}{U}")
        .Type("Instant")
        .Text("Draw three cards.")
        .FlavorText("\"Brute force can sometimes kick down a locked door, but knowledge is a skeleton key.\"")
        .Cast(p =>
        {
          p.Effect = () => new DrawCards(3);
        });
    }
  }
}
