namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class DelusionsOfMediocrity : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Delusions of Mediocrity")
        .ManaCost("{3}{U}")
        .Type("Enchantment")
        .Text(
          "When Delusions of Mediocrity enters the battlefield, you gain 10 life.{EOL}When Delusions of Mediocrity leaves the battlefield, you lose 10 life.")
        .FlavorText("When nothing but second best will do.")
        .TriggeredAbility(p =>
          {
            p.Text = "When Delusions of Mediocrity enters the battlefield, you gain 10 life.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new ControllerGainsLife(10);
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When Delusions of Mediocrity leaves the battlefield, you lose 10 life.";
            p.Trigger(new OnZoneChanged(from: Zone.Battlefield));
            p.Effect = () => new ControllerLoosesLife(10);
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}