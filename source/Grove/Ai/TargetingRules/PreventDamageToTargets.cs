namespace Grove.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay.Card;
  using Gameplay.Player;
  using Gameplay.States;
  using Gameplay.Targeting;

  public class PreventDamageToTargets : TargetingRule
  {
    private readonly int _amount;

    private PreventDamageToTargets() {}

    public PreventDamageToTargets(int amount = int.MaxValue)
    {
      _amount = amount;
    }

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      if (!Stack.IsEmpty)
      {
        return PreventDamageTopSpellWillDealToCreatureOrPlayer(p);
      }

      if (Turn.Step == Step.DeclareBlockers)
      {
        return p.Controller.IsActive
          ? PreventDamageBlockerWillDealToAttacker(p)
          : PreventDamageAttackerWillDealToPlayerOrBlocker(p);
      }

      return None<Targets>();
    }

    private IEnumerable<Targets> PreventDamageAttackerWillDealToPlayerOrBlocker(TargetingRuleParameters p)
    {
      var playerCandidate = p.Candidates<Player>(ControlledBy.SpellOwner)
        .Where(x => Combat.WillAnyAttackerDealDamageToDefender());

      var creatureCandidates = p.Candidates<Card>(ControlledBy.SpellOwner)
        .Where(x => x.IsBlocker)
        .Where(x =>
          {
            var prevented = QuickCombat.GetAmountOfDamageThatNeedsToBePreventedToSafeBlockerFromDying(
              blocker: x,
              attacker: Combat.GetAttacker(x));


            return 0 < prevented && prevented <= _amount;
          })
        .OrderByDescending(x => x.Score);

      var candidates = new List<ITarget>();
      candidates.AddRange(playerCandidate);
      candidates.AddRange(creatureCandidates);

      return Group(candidates, p.MinTargetCount(), p.MaxTargetCount());
    }

    private IEnumerable<Targets> PreventDamageBlockerWillDealToAttacker(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.SpellOwner)
        .Where(x => x.IsAttacker)
        .Where(x =>
          {
            var prevented = QuickCombat.GetAmountOfDamageThatNeedsToBePreventedToSafeAttackerFromDying(
              attacker: x,
              blockers: Combat.GetBlockers(x));

            return 0 < prevented && prevented <= _amount;
          })
        .OrderByDescending(x => x.Score)
        .ToList();

      return Group(candidates, p.MinTargetCount(), p.MaxTargetCount());
    }

    private IEnumerable<Targets> PreventDamageTopSpellWillDealToCreatureOrPlayer(TargetingRuleParameters p)
    {
      var playerCandidate = p.Candidates<Player>(ControlledBy.SpellOwner)
        .Where(x => Stack.GetDamageTopSpellWillDealToPlayer(x) > 0);

      var creatureCandidates = p.Candidates<Card>(ControlledBy.SpellOwner)
        .Where(x =>
          {
            var damageToCreature = Stack.GetDamageTopSpellWillDealToCreature(x);
            return (damageToCreature >= x.Life) && (damageToCreature - _amount < x.Life);
          })
        .OrderByDescending(x => x.Score);

      var candidates = new List<ITarget>();
      candidates.AddRange(playerCandidate);
      candidates.AddRange(creatureCandidates);

      return Group(candidates, p.MinTargetCount(), p.MaxTargetCount());
    }
  }
}