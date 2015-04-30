namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class BloodfellCaves : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Bloodfell Caves")
        .Type("Land")
        .Text("Bloodfell Caves enters the battlefield tapped.{EOL}When Bloodfell Caves enters the battlefield, you gain 1 life.{EOL}{T}: Add {B} or {R} to your mana pool.")
        .Cast(p => p.Effect = () => new CastPermanent(tap: true))
        .ManaAbility(p =>
        {
          p.Text = "{T}: Add {B} or {R} to your mana pool.";
          p.ManaAmount(Mana.Colored(isBlack: true, isRed: true));
        })
        .TriggeredAbility(p =>
        {
          p.Text = "When Bloodfell Caves enters the battlefield, you gain 1 life.";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
          p.Effect = () => new ChangeLife(1, whos: P(e => e.Controller));
        });
    }
  }
}
