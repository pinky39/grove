namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Modifiers;

  public class AeronautTinkerer : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Aeronaut Tinkerer")
        .ManaCost("{2}{U}")
        .Type("Creature — Human Artificer")
        .Text(
          "Aeronaut Tinkerer has flying as long as you control an artifact.{I}(It can't be blocked except by creatures with flying or reach.){/I}")
        .FlavorText("\"All tinkerers have their heads in the clouds. I don't intend to stop there.\"")
        .Power(2)
        .Toughness(3)
        .StaticAbility(p => 
          p.Modifier(() => new AddStaticAbilityAsLongAsYouControlPermanent(Static.Flying, c => c.Is().Artifact)));
    }
  }
}