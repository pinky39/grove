namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class RuggedHighlands : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Rugged Highlands")
        .Type("Land")
        .Text("Rugged Highlands enters the battlefield tapped.{EOL}When Rugged Highlands enters the battlefield, you gain 1 life.{EOL}{T}: Add {R} or {G} to your mana pool.")
        .Cast(p => p.Effect = () => new CastPermanent(tap: true))
        .ManaAbility(p =>
        {
          p.Text = "{T}: Add {R} or {G} to your mana pool.";
          p.ManaAmount(Mana.Colored(isGreen: true, isRed: true));
        })
        .TriggeredAbility(p =>
        {
          p.Text = "When Rugged Highlands enters the battlefield, you gain 1 life.";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
          p.Effect = () => new ChangeLife(1, whos: P(e => e.Controller));
        });
    }
  }
}
