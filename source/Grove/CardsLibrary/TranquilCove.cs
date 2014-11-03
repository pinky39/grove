namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class TranquilCove : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Tranquil Cove")
        .Type("Land")
        .Text("Tranquil Cove enters the battlefield tapped.{EOL}When Tranquil Cove enters the battlefield, you gain 1 life.{EOL}{T}: Add {W} or {U} to your mana pool.")
        .Cast(p => p.Effect = () => new CastPermanent(tap: true))
        .ManaAbility(p =>
        {
          p.Text = "{T}: Add {W} or {U} to your mana pool.";
          p.ManaAmount(Mana.Colored(isBlue: true, isWhite: true));
        })
        .TriggeredAbility(p =>
        {
          p.Text = "When Tranquil Cove enters the battlefield, you gain 1 life.";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
          p.Effect = () => new ChangeLife(1, yours: true);
        });
    }
  }
}
