namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class SwiftwaterCliffs : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Swiftwater Cliffs")
        .Type("Land")
        .Text("Swiftwater Cliffs enters the battlefield tapped.{EOL}When Swiftwater Cliffs enters the battlefield, you gain 1 life.{EOL}{T}: Add {U} or {R} to your mana pool.")
        .Cast(p => p.Effect = () => new CastPermanent(tap: true))
        .ManaAbility(p =>
        {
          p.Text = "{T}: Add {U} or {R} to your mana pool.";
          p.ManaAmount(Mana.Colored(isBlue: true, isRed: true));
        })
        .TriggeredAbility(p =>
        {
          p.Text = "When Swiftwater Cliffs enters the battlefield, you gain 1 life.";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
          p.Effect = () => new ChangeLife(1, yours: true);
        });
    }
  }
}
