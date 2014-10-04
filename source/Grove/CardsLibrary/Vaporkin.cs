namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class Vaporkin : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
          .Named("Vaporkin")
          .ManaCost("{1}{U}")
          .Type("Creature - Elemental")
          .Text("{Flying}{EOL}Vaporkin can block only creatures with flying.")
          .FlavorText("\"Mists are carefree. They drift where they will, unencumbered by rocks and river beds.\"{EOL}—Thrasios, triton hero")
          .Power(2)
          .Toughness(1)
          .SimpleAbilities(Static.Flying, Static.CanBlockOnlyCreaturesWithFlying);
    }
  }
}
