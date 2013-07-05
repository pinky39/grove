namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.Misc;

  public class YavimayaWurm : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Yavimaya Wurm")
        .ManaCost("{4}{G}{G}")
        .Type("Creature Wurm")
        .Text("{Trample}")
        .FlavorText(
          "When the gorilla playfully grabbed the wurm's tail, the wurm doubled back and playfully ate the gorilla's head.")
        .Power(6)
        .Toughness(4)
        .SimpleAbilities(Static.Trample);
    }
  }
}