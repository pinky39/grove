namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class SiegeRhino : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Siege Rhino")
        .ManaCost("{1}{W}{B}{G}")
        .Type("Creature - Rhino")
        .Text("{Trample}{EOL}When Siege Rhino enters the battlefield, each opponent loses 3 life and you gain 3 life.")
        .FlavorText("The mere approach of an Abzan war beast is enough to send enemies fleeing in panic.")
        .Power(4)
        .Toughness(5)
        .SimpleAbilities(Static.Trample)
        .TriggeredAbility(p =>
        {
          p.Text = "When Siege Rhino enters the battlefield, each opponent loses 3 life and you gain 3 life.";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
          p.Effect = () => new CompoundEffect(
              new ChangeLife(amount: -3, opponents: true),
              new ChangeLife(amount: 3, yours: true));
        });
    }
  }
}
