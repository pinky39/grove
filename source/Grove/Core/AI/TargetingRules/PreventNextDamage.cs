namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class PreventNextDamage
  {
    private readonly int _amount;
    private readonly Game _game;
    private readonly TargetingRuleParameters _p;

    private PreventNextDamage(int amount, TargetingRuleParameters p, Game game)
    {
      _amount = amount;
      _p = p;
      _game = game;
    }

    public static IList<ITarget> GetCandidates(int amount, TargetingRuleParameters p, Game game)
    {
      var preventNextDamage = new PreventNextDamage(amount, p, game);
      return preventNextDamage.GetCandidates();
    }

    private IList<ITarget> GetCandidates()
    {
      if (!_game.Stack.IsEmpty)
      {
        return PreventDamageTopSpellWillDealToCreatureOrPlayer();
      }

      if (_game.Turn.Step == Step.DeclareBlockers)
      {
        return _p.Controller.IsActive
          ? PreventDamageBlockerWillDealToAttacker()
          : PreventDamageAttackerWillDealToPlayerOrBlocker();
      }

      return new ITarget[] {};
    }

    private List<ITarget> PreventDamageAttackerWillDealToPlayerOrBlocker()
    {
      var playerCandidate = _p.Candidates<Player>(ControlledBy.SpellOwner, selector: c => c.Effect)
        .Where(x => _game.Combat.WillAnyAttackerDealDamageToDefender());

      var creatureCandidates = _p.Candidates<Card>(ControlledBy.SpellOwner, selector: c => c.Effect)
        .Where(x => x.IsBlocker)
        .Where(x =>
          {
            var prevented = QuickCombat.GetAmountOfDamageThatNeedsToBePreventedToSafeBlockerFromDying(
              blocker: x,
              attacker: _game.Combat.GetAttacker(x));


            return 0 < prevented && prevented <= _amount;
          })
        .OrderByDescending(x => x.Score);

      var candidates = new List<ITarget>();
      candidates.AddRange(playerCandidate);
      candidates.AddRange(creatureCandidates);

      return candidates;
    }

    private List<ITarget> PreventDamageBlockerWillDealToAttacker()
    {
      var candidates = _p.Candidates<Card>(ControlledBy.SpellOwner, selector: c => c.Effect)
        .Where(x => x.IsAttacker)
        .Where(x =>
          {
            var prevented = QuickCombat.GetAmountOfDamageThatNeedsToBePreventedToSafeAttackerFromDying(
              attacker: x,
              blockers: _game.Combat.GetBlockers(x));

            return 0 < prevented && prevented <= _amount;
          })
        .OrderByDescending(x => x.Score);

      return candidates.Cast<ITarget>().ToList();
    }

    private List<ITarget> PreventDamageTopSpellWillDealToCreatureOrPlayer()
    {
      var playerCandidate = _p.Candidates<Player>(ControlledBy.SpellOwner, selector: c => c.Effect)
        .Where(x => _game.Stack.GetDamageTopSpellWillDealToPlayer(x) > 0);

      var creatureCandidates = _p.Candidates<Card>(ControlledBy.SpellOwner, selector: c => c.Effect)
        .Where(x =>
          {
            var damageToCreature = _game.Stack.GetDamageTopSpellWillDealToCreature(x);
            return (damageToCreature >= x.Life) && (damageToCreature - _amount < x.Life);
          })
        .OrderByDescending(x => x.Score);

      var candidates = new List<ITarget>();
      candidates.AddRange(playerCandidate);
      candidates.AddRange(creatureCandidates);

      return candidates;
    }
  }
}