namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Misc;

  public class YavimayaScion : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Yavimaya Scion")
        .ManaCost("{4}{G}")
        .Type("Creature Treefolk")
        .Text("{Protection from artifacts}")
        .FlavorText("Each time the saw blade bit, the tree spat it out.")
        .Power(4)
        .Toughness(4)
        .Protections("artifact");
    }
  }
}