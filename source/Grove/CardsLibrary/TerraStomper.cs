namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;

  public class TerraStomper : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Terra Stomper")
        .ManaCost("{3}{G}{G}{G}")
        .Type("Creature - Beast")
        .Text("Terra Stomper can't be countered.{EOL}{Trample}")
        .FlavorText("Its footfalls cause violent earthquakes, hurtling boulders, and unseasonable dust storms.")
        .Power(8)
        .Toughness(8)
        .SimpleAbilities(Static.Trample)
        .Cast(p =>
        {
          p.Effect = () => new PutIntoPlay(putIntoBattlefield: true){CanBeCountered = false};
        });
    }
  }
}
