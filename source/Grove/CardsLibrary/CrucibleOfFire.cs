namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Modifiers;

  public class CrucibleOfFire : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Crucible of Fire")
        .ManaCost("{3}{R}")
        .Type("Enchantment")
        .Text("Dragon creatures you control get +3/+3.")
        .FlavorText("\"The dragon is a perfect marriage of power and the will to use it.\"—Sarkhan Vol")
        .ContinuousEffect(p =>
        {
          p.CardFilter = (card, effect) => card.Is("Dragon") && card.Controller == effect.Source.Controller;
          p.Modifier = () => new AddPowerAndToughness(3, 3);
        });
    }
  }
}
