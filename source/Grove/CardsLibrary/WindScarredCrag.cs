namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class WindScarredCrag : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Wind-Scarred Crag")
        .Type("Land")
        .Text("Wind-Scarred Crag enters the battlefield tapped.{EOL}When Wind-Scarred Crag enters the battlefield, you gain 1 life.{EOL}{T}: Add {R} or {W} to your mana pool.")
        .Cast(p => p.Effect = () => new CastPermanent(tap: true))
        .ManaAbility(p =>
        {
          p.Text = "{T}: Add {R} or {W} to your mana pool.";
          p.ManaAmount(Mana.Colored(isWhite: true, isRed: true));
        })
        .TriggeredAbility(p =>
        {
          p.Text = "When Wind-Scarred Crag enters the battlefield, you gain 1 life.";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
          p.Effect = () => new ChangeLife(1, yours: true);
        });
    }
  }
}
