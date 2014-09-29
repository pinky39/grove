namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;

  public class CarrionCrow : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Carrion Crow")
        .ManaCost("{2}{B}")
        .Type("Creature - Zombie Bird")
        .Text(
          "{Flying}{I}(This creature can't be blocked except by creatures with flying or reach.){/I}{EOL}Carrion Crow enters the battlefield tapped.")
        .FlavorText("When carrion feeds on carrion, dark days approach.")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.Flying)
        .Cast(p => { p.Effect = () => new PutIntoPlay(tap: true); });
    }
  }
}