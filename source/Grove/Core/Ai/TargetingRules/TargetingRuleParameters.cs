namespace Grove.Core.Ai.TargetingRules
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Targeting;

  public class TargetingRuleParameters : GameObject
  {
    private readonly TargetsCandidates _candidates;
    private readonly Ai.ActivationContext _context;

    public TargetingRuleParameters(TargetsCandidates candidates, Ai.ActivationContext context, Game game)
    {
      _candidates = candidates;
      _context = context;
      Game = game;
    }

    public Player Controller { get { return _context.Card.Controller; } }
    public bool CanCancel { get { return _context.CanCancel; } }
    public int? X { get { return _context.X; } }
    public int MaxX { get { return _context.MaxX.GetValueOrDefault(); } }
    public int EffectTargetTypeCount { get { return _context.Selector.Effect.Count; } }
    public Card Card { get { return _context.Card; } }
    public bool DistributeDamage { get { return _context.DistributeDamage; } }

    public IEnumerable<T> Candidates<T>(
      ControlledBy controlledBy = ControlledBy.Any,
      int selectorIndex = 0,
      Func<TargetsCandidates, IList<TargetCandidates>> selector = null)
      where T : ITarget
    {
      TargetCandidates candidates = null;

      if (selector == null)
      {
        candidates = _candidates.HasCost
          ? _candidates.Cost[selectorIndex]
          : _candidates.Effect[selectorIndex];
      }
      else
      {
        candidates = selector(_candidates)[selectorIndex];
      }

      switch (controlledBy)
      {
        case (ControlledBy.Opponent):
          {
            return candidates
              .Where(x => x.Controller() == Controller.Opponent && x is T)
              .Select(x => (T) x);
          }
        case (ControlledBy.SpellOwner):
          {
            return candidates
              .Where(x => x.Controller() == Controller && x is T)
              .Select(x => (T) x);
          }
      }

      return candidates
        .Where(x => x is T)
        .Select(x => (T) x);
    }

    public int MinTargetCount()
    {
      return _context.Selector.GetMinEffectTargetCount();
    }

    public int MaxTargetCount()
    {
      return _context.Selector.GetMaxEffectTargetCount();
    }
  }
}