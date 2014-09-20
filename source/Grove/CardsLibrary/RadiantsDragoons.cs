namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class RadiantsDragoons : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Radiant's Dragoons")
        .ManaCost("{3}{W}")
        .Type("Creature Human Soldier")
        .Text("{Echo} {3}{W}{EOL}When Radiant's Dragoons enters the battlefield, you gain 5 life.")
        .Power(2)
        .Toughness(5)
        .Echo("{3}{W}")
        .TriggeredAbility(p =>
          {
            p.Text = "When Radiant's Dragoons enters the battlefield, you gain 5 life.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new YouGainLife(5);
          });
    }
  }
}