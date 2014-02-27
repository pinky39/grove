namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class BlanchwoodTreefolk : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Blanchwood Treefolk")
        .ManaCost("{4}{G}")
        .Type("Creature - Treefolk")
        .FlavorText(
          "The massive Argivian attack on their rooted kindred was a declaration of war to the treefolk.")
        .Power(4)
        .Toughness(5);
    }
  }
}