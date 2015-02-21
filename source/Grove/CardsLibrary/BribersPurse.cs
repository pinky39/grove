namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.CostRules;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;

  public class BribersPurse : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Briber's Purse")
        .ManaCost("{0}").HasXInCost()
        .Type("Artifact")
        .Text("Briber's Purse enters the battlefield with X gem counters on it.{EOL}{1},{T}, Remove a gem counter from Briber's Purse: Target creature can't attack or block this turn.")
        .FlavorText("Victory is certain. The price, negotiable.")
        .Cast(p => p.CostRule(new XIsAvailableMana()) )
        .TriggeredAbility(p =>
        {
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
          p.Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new SimpleCounter(CounterType.Gem), Value.PlusX));
          p.UsesStack = false;
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{1},{T}, Remove a gem counter from Briber's Purse: Target creature can't attack or block this turn.";
          p.Cost = new AggregateCost(
            new PayMana(1.Colorless()),
            new Tap(),
            new RemoveCounters(CounterType.Gem, count: 1));
          p.Effect = () => new ApplyModifiersToTargets(
            () => new AddStaticAbility(Static.CannotBlock) { UntilEot = true },
            () => new AddStaticAbility(Static.CannotAttack) { UntilEot = true }).SetTags(EffectTag.CombatDisabler);

          p.TimingRule(new OnFirstMain());
          p.TargetingRule(new EffectCannotBlockAttack());
        });
    }
  }
}
