namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class SultaiFlayer : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
          .Named("Sultai Flayer")
          .ManaCost("{3}{G}")
          .Type("Creature — Naga Shaman")
          .Text("Whenever a creature you control with toughness 4 or greater dies, you gain 4 life.")
          .FlavorText("\"You can have the body, necromancer. I just want the skin.\"")
          .Power(3)
          .Toughness(4)
          .TriggeredAbility(p =>
          {
            p.Text = "Whenever a creature you control with toughness 4 or greater dies, you gain 4 life.";
            p.Trigger(new OnZoneChanged(
              from: Zone.Battlefield,
              to: Zone.Graveyard,
              selector: (c, ctx) => c.Controller == ctx.You && c.Is().Creature && c.Toughness >= 4));
            p.Effect = () => new ChangeLife(4, whos: P(e => e.Controller));
          });
    }
  }
}
