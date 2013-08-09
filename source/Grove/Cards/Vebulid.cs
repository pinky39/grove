namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Counters;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.States;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class Vebulid : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Vebulid")
        .ManaCost("{B}")
        .Type("Creature Horror")
        .Text(
          "Vebulid enters the battlefield with a +1/+1 counter on it.{EOL}At the beginning of your upkeep, you may put a +1/+1 counter on Vebulid.{EOL}When Vebulid attacks or blocks, destroy it at end of combat.")
        .Power(0)
        .Toughness(0)
        .TriggeredAbility(p =>
          {
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new PowerToughness(1, 1), 1));
            p.UsesStack = false;
          })
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you may put a +1/+1 counter on Vebulid.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new PowerToughness(1, 1), 1));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When Vebulid attacks or blocks, destroy it at end of combat.";
            
            p.Trigger(new OnStepStart(Step.EndOfCombat, activeTurn: true, passiveTurn: true)
              {Condition = (t, g) => g.Turn.Events.HasAttacked(t.OwningCard) || g.Turn.Events.HasBlocked(t.OwningCard)});
            
            p.Effect = () => new DestroyOwner();
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}