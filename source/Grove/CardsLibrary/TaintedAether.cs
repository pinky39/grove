namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Events;
  using Grove.AI.TimingRules;
  using Grove.Triggers;

  public class TaintedAether : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Tainted Aether")
        .ManaCost("{2}{B}{B}")
        .Type("Enchantment")
        .Text("Whenever a creature enters the battlefield, its controller sacrifices a creature or land.")
        .FlavorText(
          "Gix despised the sylvan morass. The gouge that the portal had torn in the forest was the only pleasing sight.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever a creature enters the battlefield, its controller sacrifices a creature or land.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield, filter: (c, a , g) => c.Is().Creature));
            p.TriggerOnlyIfOwningCardIsInPlay = true;

            p.Effect = () => new PlayerSacrificePermanents(
              count: 1,
              player: P(e => e.TriggerMessage<ZoneChangedEvent>().Controller),
              filter: c => c.Is().Creature || c.Is().Land,
              text: "Sacrifice a creature or a land.");
          });
    }
  }
}