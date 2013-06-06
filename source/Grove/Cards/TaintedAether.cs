namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Messages;
  using Gameplay.Misc;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class TaintedAether : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Tainted Aether")
        .ManaCost("{2}{B}{B}")
        .Type("Enchantment")
        .Text("Whenever a creature enters the battlefield, its controller sacrifices a creature or land.")
        .FlavorText(
          "Gix despised the sylvan morass. The gouge that the portal had torn in the forest was the only pleasing sight.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever a creature enters the battlefield, its controller sacrifices a creature or land.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield, filter: (a, c) => c.Is().Creature));
            p.TriggerOnlyIfOwningCardIsInPlay = true;

            p.Effect = () => new PlayerSacrificePermanents(
              count: 1,
              player: P(e => e.TriggerMessage<ZoneChanged>().Controller),
              filter: c => c.Is().Creature || c.Is().Land,
              text: "Sacrifice a creature or a land.");
          });
    }
  }
}