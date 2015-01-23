namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class TempleOfSilence : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Temple of Silence")
        .Type("Land")
        .Text("Temple of Silence enters the battlefield tapped.{EOL}When Temple of Silence enters the battlefield, scry 1.{I}(Look at the top card of your library. You may put that card on the bottom of your library.){/I}{EOL}{T}: Add {W} or {B} to your mana pool.")
        .Cast(p => p.Effect = () => new CastPermanent(tap: true))
        .ManaAbility(p =>
        {
          p.Text = "{T}: Add {W} or {B} to your mana pool.";
          p.ManaAmount(Mana.Colored(isBlack: true, isWhite: true));
        })
        .TriggeredAbility(p =>
        {
          p.Text = "When Temple of Silence enters the battlefield, scry 1.";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
          p.Effect = () => new Scry(1);
        });
    }
  }
}
