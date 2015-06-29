namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class StaffOfTheDeathMagus : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Staff of the Death Magus")
        .ManaCost("{3}")
        .Type("Artifact")
        .Text("Whenever you cast a black spell or a Swamp enters the battlefield under your control, you gain 1 life.")
        .FlavorText("A symbol of ambition in ruthless times.")
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever you cast a black spell or a Swamp enters the battlefield under your control, you gain 1 life.";

            p.Trigger(new OnCastedSpell((c, ctx) => 
              c.HasColor(CardColor.Black) && c.Controller == ctx.You));
            
            p.Trigger(new OnZoneChanged(
              to: Zone.Battlefield, 
              selector: (c, ctx) => c.Is("swamp") && c.Controller == ctx.You));

            p.Effect = () => new ChangeLife(amount: 1, whos: P(e => e.Controller));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}