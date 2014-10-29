namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class BlossomingSands : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Blossoming Sands")
        .Type("Land")
        .Text("Blossoming Sands enters the battlefield tapped.{EOL}When Blossoming Sands enters the battlefield, you gain 1 life.{EOL}{T}: Add {G} or {W} to your mana pool.")
        .Cast(p => p.Effect = () => new CastPermanent(tap: true))
        .ManaAbility(p =>
        {
          p.Text = "{T}: Add {G} or {W} to your mana pool.";
          p.ManaAmount(Mana.Colored(isGreen: true, isWhite: true));
        })
        .TriggeredAbility(p =>
        {
          p.Text = "When Blossoming Sands enters the battlefield, you gain 1 life.";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
          p.Effect = () => new ChangeLife(1, yours: true);
        });
    }
  }
}
