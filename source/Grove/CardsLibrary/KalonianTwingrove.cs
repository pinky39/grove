namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Modifiers;
  using Triggers;

  public class KalonianTwingrove : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Kalonian Twingrove")
        .ManaCost("{5}{G}")
        .Type("Creature — Treefolk Warrior")
        .Text("Kalonian Twingrove's power and toughness are each equal to the number of Forests you control.{EOL}When Kalonian Twingrove enters the battlefield, put a green Treefolk Warrior creature token onto the battlefield with \"This creature's power and toughness are each equal to the number of Forests you control.\"")
        .Power(0)
        .Toughness(0)
        .StaticAbility(p =>
        {
          p.Modifier(() => new ModifyPowerToughnessForEachPermanent(
            power: 1,
            toughness: 1,
            filter: (c, _) => c.Is("Forest"),
            modifier: () => new IntegerSetter()));

          p.EnabledInAllZones = true;
        })
        .TriggeredAbility(p =>
        {
          p.Text = "When Kalonian Twingrove enters the battlefield, put a green Treefolk Warrior creature token onto the battlefield with \"This creature's power and toughness are each equal to the number of Forests you control.\"";

            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

            p.Effect = () => new CreateTokens(
              count: 1,
              token: Card
                .Named("Treefolk Warrior")
                .Power(0)
                .Toughness(0)
                .Type("Token Creature - Treefolk Warrior")
                .Text("This creature's power and toughness are each equal to the number of Forests you control.")
                .Colors(CardColor.Green)
                .StaticAbility(ap =>
                {
                  ap.Modifier(() => new ModifyPowerToughnessForEachPermanent(
                    power: 1,
                    toughness: 1,
                    filter: (c, _) => c.Is("Forest"),
                    modifier: () => new IntegerSetter()));

                  ap.EnabledInAllZones = true;
                }));
        });
    }
  }
}
