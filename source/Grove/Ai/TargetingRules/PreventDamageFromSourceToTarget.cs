namespace Grove.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Gameplay.Card;
  using Gameplay.States;
  using Gameplay.Targeting;

  public class PreventDamageFromSourceToTarget : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var targetPicks = new List<ITarget>();
      var sourcePicks = new List<ITarget>();

      if (Stack.IsEmpty == false)
      {
        PreventDamageTopSpellWillDealToPlayer(p, targetPicks, sourcePicks);
        PreventDamageTopSpellWillDealToCreature(p, targetPicks, sourcePicks);
      }

      if (Turn.Step == Step.DeclareBlockers)
      {
        if (p.Controller.IsActive == false)
        {
          PreventDamageAttackerWillDealToPlayer(p, targetPicks, sourcePicks);
          PreventDamageAttackerWillDealToBlocker(p, targetPicks, sourcePicks);
        }
        else
        {
          PreventDamageBlockerWillDealToAttacker(p, targetPicks, sourcePicks);
        }
      }

      return Group(sourcePicks, targetPicks);
    }

    private void PreventDamageBlockerWillDealToAttacker(TargetingRuleParameters p, List<ITarget> targetPicks,
      List<ITarget> sourcePicks)
    {
      var blockerAttackerPair = p.Candidates<Card>(selectorIndex: 1)
        .Where(x => x.IsAttacker)
        .Select(x => new
          {
            Target = x,
            Source =
              QuickCombat.GetBlockerThatDealsLeathalDamageToAttacker(
                attacker: x,
                blockers: Combat.GetBlockers(x.Card()))
          })
        .Where(x => x.Source != null)
        .OrderByDescending(x => x.Target.Score)
        .FirstOrDefault();

      if (blockerAttackerPair != null)
      {
        targetPicks.Add(blockerAttackerPair.Target);
        sourcePicks.Add(blockerAttackerPair.Source);
      }
    }

    private void PreventDamageAttackerWillDealToBlocker(TargetingRuleParameters p, List<ITarget> targetPicks,
      List<ITarget> sourcePicks)
    {
      var blockerAttackerPair = p.Candidates<Card>(selectorIndex: 1)
        .Where(x => x.IsBlocker)
        .Select(x => new
          {
            Target = x,
            Source =
              QuickCombat.GetAttackerThatDealsLeathalDamageToBlocker(
                blocker: x,
                attacker: Combat.GetAttacker(x))
          })
        .Where(x => x.Source != null)
        .OrderByDescending(x => x.Target.Score)
        .FirstOrDefault();

      if (blockerAttackerPair != null)
      {
        targetPicks.Add(blockerAttackerPair.Target);
        sourcePicks.Add(blockerAttackerPair.Source);
      }
    }

    private void PreventDamageAttackerWillDealToPlayer(TargetingRuleParameters p, List<ITarget> targetPicks,
      List<ITarget> sourcePicks)
    {
      var attacker = Combat.GetAttackerWhichWillDealGreatestDamageToDefender();

      if (attacker != null)
      {
        targetPicks.Add(p.Controller);
        sourcePicks.Add(attacker);
      }
    }

    private void PreventDamageTopSpellWillDealToCreature(TargetingRuleParameters p, List<ITarget> targetPicks,
      List<ITarget> sourcePicks)
    {
      var creatureKilledByTopSpell = p.Candidates<Card>(selectorIndex: 1)
        .Where(x => Stack.CanBeDealtLeathalDamageByTopSpell(x))
        .OrderByDescending(x => x.Score)
        .FirstOrDefault();

      if (creatureKilledByTopSpell != null)
      {
        targetPicks.Add(creatureKilledByTopSpell);
        sourcePicks.Add(Stack.TopSpell);
      }
    }

    private void PreventDamageTopSpellWillDealToPlayer(TargetingRuleParameters p, List<ITarget> targetPicks,
      List<ITarget> sourcePicks)
    {
      var damageToPlayer = Stack.GetDamageTopSpellWillDealToPlayer(p.Controller);

      if (damageToPlayer > 0)
      {
        targetPicks.Add(p.Controller);
        sourcePicks.Add(Stack.TopSpell);
      }
    }
  }
}