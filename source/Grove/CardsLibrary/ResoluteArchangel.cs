namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class ResoluteArchangel : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Resolute Archangel")
        .ManaCost("{5}{W}{W}")
        .Type("Creature - Angel")
        .Text(
          "{Flying}{EOL}When Resolute Archangel enters the battlefield, if your life total is less than your starting life total, it becomes equal to your starting life total.")
        .FlavorText("Cut it down, bury it in snow, put it to the torch. The rose will still bloom again.")
        .Power(4)
        .Toughness(4)
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Resolute Archangel enters the battlefield, if your life total is less than your starting life total, it becomes equal to your starting life total.";

            p.Trigger(new OnZoneChanged(
              to: Zone.Battlefield,
              filter: (c, a, g) => c.Controller.Life < 20));

            p.Effect = () => new YourLifeBecomesEqual(20);
          });
    }
  }
}