namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class JungleHollow : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Jungle Hollow")
        .Type("Land")
        .Text("Jungle Hollow enters the battlefield tapped.{EOL}When Jungle Hollow enters the battlefield, you gain 1 life.{EOL}{T}: Add {B} or {G} to your mana pool.")
        .Cast(p => p.Effect = () => new CastPermanent(tap: true))
        .ManaAbility(p =>
        {
          p.Text = "{T}: Add {B} or {G} to your mana pool.";
          p.ManaAmount(Mana.Colored(isBlack: true, isGreen: true));
        })
        .TriggeredAbility(p =>
        {
          p.Text = "When Jungle Hollow enters the battlefield, you gain 1 life.";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
          p.Effect = () => new ChangeLife(1, yours: true);
        });
    }
  }
}
