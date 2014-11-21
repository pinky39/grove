namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Modifiers;

  public class UrborgTombOfYawgmoth : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Urborg, Tomb of Yawgmoth")
        .Type("Legendary Land")
        .Text("Each land is a Swamp in addition to its other land types.")
        .FlavorText("\"Yawgmoth's corpse is a wound in the universe. His foul blood seeps out, infecting the land with his final curse.\"{EOL}—Lord Windgrace")
        .ContinuousEffect(p =>
        {
          p.Modifier = () => new ChangeBasicLandSubtype("Swamp", replace: false);
          p.CardFilter = (card, effect) => card.Is().Land;
        });
    }
  }
}
