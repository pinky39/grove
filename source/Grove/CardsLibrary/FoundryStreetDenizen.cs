namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using Effects;
  using Modifiers;
  using Triggers;

  public class FoundryStreetDenizen : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
          .Named("Foundry Street Denizen")
          .ManaCost("{R}")
          .Type("Creature — Goblin Warrior")
          .Text("Whenever another red creature enters the battlefield under your control, Foundry Street Denizen gets +1/+0 until end of turn.")
          .FlavorText("After the Foundry Street riot, Arrester Hulbein tried to ban bludgeons. Which, inevitably, resulted in another riot.")
          .Power(1)
          .Toughness(1)
          .TriggeredAbility(p =>
          {
            p.Text = "Whenever another red creature enters the battlefield under your control, Foundry Street Denizen gets +1/+0 until end of turn.";

            p.Trigger(new OnZoneChanged(
              to: Zone.Battlefield,
              selector: (c, ctx) => c.Is().Creature && c.HasColor(CardColor.Red) && ctx.You == c.Controller && ctx.OwningCard != c));

            p.TriggerOnlyIfOwningCardIsInPlay = true;

            p.Effect = () => new ApplyModifiersToSelf(
              () => new AddPowerAndToughness(1, 0) { UntilEot = true })
              .SetTags(EffectTag.IncreasePower);
          });
    }
  }
}
