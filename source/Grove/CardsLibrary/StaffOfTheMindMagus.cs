namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class StaffOfTheMindMagus : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Staff of the Mind Magus")
        .ManaCost("{3}")
        .Type("Artifact")
        .Text("Whenever you cast a blue spell or an Island enters the battlefield under your control, you gain 1 life.")
        .FlavorText("A symbol of sagacity in bewildering times.")
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever you cast a blue spell or an Island enters the battlefield under your control, you gain 1 life.";

            p.Trigger(new OnCastedSpell((c, ctx) =>
              c.HasColor(CardColor.Blue) && c.Controller == ctx.You));

            p.Trigger(new OnZoneChanged(
              to: Zone.Battlefield,
              selector: (c, ctx) =>
                c.Is("Island") && c.Controller == ctx.You));

            p.Effect = () => new ChangeLife(amount: 1, whos: P(e => e.Controller));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}