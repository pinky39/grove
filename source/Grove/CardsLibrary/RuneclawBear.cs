namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class RuneclawBear : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Runeclaw Bear")
        .ManaCost("{1}{G}")
        .Type("Creature - Bear")
        .FlavorText(
          "The magic of the elves leaves its mark on the forest. The magic of the forest leaves its mark on the animals who live there. The animals of the forest leave their mark on all who trespass.")
        .Power(2)
        .Toughness(2);
    }
  }
}