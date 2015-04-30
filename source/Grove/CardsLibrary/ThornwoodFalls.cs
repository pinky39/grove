namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class ThornwoodFalls : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Thornwood Falls")
        .Type("Land")
        .Text("Thornwood Falls enters the battlefield tapped.{EOL}When Thornwood Falls enters the battlefield, you gain 1 life.{EOL}{T}: Add {G} or {U} to your mana pool.")
        .Cast(p => p.Effect = () => new CastPermanent(tap: true))
        .ManaAbility(p =>
        {
          p.Text = "{T}: Add {G} or {U} to your mana pool.";
          p.ManaAmount(Mana.Colored(isGreen: true, isBlue: true));
        })
        .TriggeredAbility(p =>
        {
          p.Text = "When Thornwood Falls enters the battlefield, you gain 1 life.";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
          p.Effect = () => new ChangeLife(1, whos: P(e => e.Controller));
        });
    }
  }
}
