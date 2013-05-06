namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Misc;

  public class TrainedArmodon : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Trained Armodon")
        .ManaCost("{1}{G}{G}")
        .Type("Creature - Elephant")
        .FlavorText("Armodons are trained to step on things - enemy things.")
        .Power(3)
        .Toughness(3);
    }
  }
}