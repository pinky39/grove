namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class AltarOfTheBrood : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Altar of the Brood")
        .ManaCost("{1}")
        .Type("Artifact")
        .Text("Whenever another permanent enters the battlefield under your control, each opponent puts the top card of his or her library into his or her graveyard.")
        .FlavorText("Supplicants offer flesh and silver, flowers and blood. The altar takes what it will, eyes gleaming with unspoken promises.")
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever another permanent enters the battlefield under your control, each opponent puts the top card of his or her library into his or her graveyard.";

          p.Trigger(new OnZoneChanged(
              to: Zone.Battlefield,
              selector: (c, ctx) =>
              {                                
                if (ctx.OwningCard == c)
                  return false;

                return ctx.You == c.Controller;
              }));

          p.Effect = () => new PlayerPutsTopCardsFromLibraryToGraveyard(P(e => e.Controller.Opponent), count: 1);
        });
    }
  }
}
