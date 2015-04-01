namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class StaffOfTheFlameMagus : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Staff of the Flame Magus")
        .ManaCost("{3}")
        .Type("Artifact")
        .Text("Whenever you cast a red spell or a Mountain enters the battlefield under your control, you gain 1 life.")
        .FlavorText("A symbol of passion in indifferent times.")
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever you cast a red spell or a Mountain enters the battlefield under your control, you gain 1 life.";

            p.Trigger(new OnCastedSpell((a, c) =>
              c.HasColor(CardColor.Red) && c.Controller == a.OwningCard.Controller));

            p.Trigger(new OnZoneChanged(
              to: Zone.Battlefield,
              selector: (c, ctx) => c.Is("mountain") && c.Controller == ctx.You
              ));

            p.Effect = () => new ChangeLife(amount: 1, yours: true);
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}