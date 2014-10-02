namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class CoralBarrier : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Coral Barrier")
        .ManaCost("{2}{U}")
        .Type("Creature - Wall")
        .Text(
          "{Defender}{I}(This creature can't attack.){/I}{EOL}When Coral Barrier enters the battlefield, put a 1/1 blue Squid creature token with islandwalk onto the battlefield.{I}(It can't be blocked as long as defending player controls an Island.){/I}")
        .FlavorText(
          "Ranger wisdom dictates that when fleeing from a moss-beast, you must stay calm, find your bearings, and run south.")
        .Power(1)
        .Toughness(3)
        .SimpleAbilities(Static.Defender)
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Coral Barrier enters the battlefield, put a 1/1 blue Squid creature token with islandwalk onto the battlefield. (It can't be blocked as long as defending player controls an Island.)";

            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

            p.Effect = () => new CreateTokens(
              count: 1,
              token: Card
                .Named("Squid")
                .Power(1)
                .Toughness(1)
                .Type("Token Creature - Squid")
                .Text("{Islandwalk}")
                .Colors(CardColor.Blue)
                .SimpleAbilities(Static.Islandwalk));
          });
    }
  }
}