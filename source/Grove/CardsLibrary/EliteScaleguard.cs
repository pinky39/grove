namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using AI.TargetingRules;
  using Effects;
  using Grove.AI.TimingRules;
  using Modifiers;
  using Triggers;

  public class EliteScaleguard : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Elite Scaleguard")
        .ManaCost("{4}{W}")
        .Type("Creature - Human Soldier")
        .Text("When Elite Scaleguard enters the battlefield, bolster 2.{I}(Choose a creature with the least toughness among creatures you control and put two +1/+1 counters on it.){/I}{EOL}Whenever a creature you control with a +1/+1 counter on it attacks, tap target creature defending player controls.")
        .Power(2)
        .Toughness(3)        
        .TriggeredAbility(p =>
        {
          p.Text = "When Elite Scaleguard enters the battlefield, bolster 2";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
          p.Effect = () => new ApplyModifiersToTargets(() => new AddCounters(
            () => new PowerToughness(1, 1), count: 2)).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);
          p.TargetSelector.AddEffect(
            trg => trg.Is.Card(c => c.Is().Creature && c.Controller.Battlefield.Creatures.All(x => x.Toughness >= c.Toughness), ControlledBy.SpellOwner).On.Battlefield());
          p.TargetingRule(new EffectOrCostRankBy(rank: c => -c.Score, controlledBy: ControlledBy.SpellOwner));
        })
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever a creature you control with a +1/+1 counter on it attacks, tap target creature defending player controls.";
          p.Trigger(new WhenACreatureAttacks(t => t.Opponent && t.AttackerHas(c => c.CountersCount(CounterType.PowerToughness) > 0)));
          p.Effect = () => new TapTargets();
          p.TargetSelector.AddEffect(trg => trg.Is.Creature(controlledBy: ControlledBy.Opponent).On.Battlefield());
          p.TargetingRule(new EffectTapCreature());
          p.TriggerOnlyIfOwningCardIsInPlay = true;
        });
    }
  }
}
