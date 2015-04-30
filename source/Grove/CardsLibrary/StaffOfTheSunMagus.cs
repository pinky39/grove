namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class StaffOfTheSunMagus : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Staff of the Sun Magus")
        .ManaCost("{3}")
        .Type("Artifact")
        .Text("Whenever you cast a white spell or a Plains enters the battlefield under your control, you gain 1 life.")
        .FlavorText("A symbol of conviction in uncertain times.")
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever you cast a white spell or a Plains enters the battlefield under your control, you gain 1 life.";

            p.Trigger(new OnCastedSpell((a, c) =>
              c.HasColor(CardColor.White) && c.Controller == a.OwningCard.Controller));

            p.Trigger(new OnZoneChanged(
              to: Zone.Battlefield,
              selector: (c, ctx) => c.Is("plains") &&
                c.Controller == ctx.You));

            p.Effect = () => new ChangeLife(amount: 1, whos: P(e => e.Controller));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}