namespace Grove.Artifical.TargetingRules
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Misc;
  using Gameplay.Targeting;
  using Infrastructure;

  public class TargetingRuleParameters : GameObject
  {
    private readonly TargetsCandidates _candidates;
    private readonly Artifical.ActivationContext _context;

    private TargetingRuleParameters() {}

    public TargetingRuleParameters(TargetsCandidates candidates, Artifical.ActivationContext context, Game game)
    {
      _candidates = candidates;
      _context = context;

      Game = game;
    }

    public Player Controller { get { return _context.Card.Controller; } }
    public int? X { get { return _context.X; } }
    public int MaxX { get { return _context.MaxX.Value.GetValueOrDefault(); } }
    public int EffectTargetTypeCount { get { return _context.Selector.Effect.Count; } }
    public Card Card { get { return _context.Card; } }
    public int DistributeAmount { get { return _context.DistributeAmount; } }
    public int MaxRepetitions { get { return _context.MaxRepetitions.Value; } }

    public bool HasCostCandidates { get { return _candidates.HasCost; } }
    public bool HasEffectCandidates { get { return _candidates.HasEffect; } }

    public IEnumerable<T> Candidates<T>(
      ControlledBy controlledBy = ControlledBy.Any,
      int selectorIndex = 0,
      Func<TargetsCandidates, IList<TargetCandidates>> selector = null)
      where T : ITarget
    {

      AssertEx.True(_candidates.HasCost || _candidates.HasEffect, 
        "No target selectors found, use AddEffect or AddCost to add them!");
                  
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
      return _context.Selector.GetMinTargetCount(X);
    }

    public int MaxTargetCount()
    {
      return _context.Selector.GetMaxTargetCount(X);
    }
  }
}