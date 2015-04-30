namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class DismalBackwater : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Dismal Backwater")
        .Type("Land")
        .Text("Dismal Backwater enters the battlefield tapped.{EOL}When Dismal Backwater enters the battlefield, you gain 1 life.{EOL}{T}: Add {U} or {B} to your mana pool.")
        .Cast(p => p.Effect = () => new CastPermanent(tap: true))
        .ManaAbility(p =>
        {
          p.Text = "{T}: Add {U} or {B} to your mana pool.";
          p.ManaAmount(Mana.Colored(isBlack: true, isBlue: true));
        })
        .TriggeredAbility(p =>
        {
          p.Text = "When Dismal Backwater enters the battlefield, you gain 1 life.";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
          p.Effect = () => new ChangeLife(1, whos: P(e => e.Controller));
        });
    }
  }
}
