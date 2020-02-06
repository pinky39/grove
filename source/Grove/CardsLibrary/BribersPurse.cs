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
        .Cast(p =>
        {
          p.CostRule(new XIsAvailableMana(-1));
          p.AfterResolve = (c, ctx) =>
          {           
              var x = ctx.X ?? 0;
              
              c.PutToBattlefield();

              var mp = new ModifierParameters
              {
                SourceEffect = ctx.Effect,
                SourceCard = c,
                X = x
              };

              var addCounters = new AddCounters(() => new SimpleCounter(CounterType.Gem), Value.PlusX);
              c.AddModifier(addCounters, mp);
          };
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

          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

          p.TimingRule(new OnStep(Step.BeginningOfCombat));
          p.TargetingRule(new EffectCannotBlockAttack());
        });
    }
  }
}
