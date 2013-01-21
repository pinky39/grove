namespace Grove.Core.Ai.TargetSelectionRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Targeting;

  public class TargetSelectionParameters : GameObject
  {
    private readonly TargetsCandidates _candidates;
    private readonly ActivationPrerequisites _prerequisites;

    public TargetSelectionParameters(TargetsCandidates candidates, ActivationPrerequisites prerequisites, Game game)
    {
      _candidates = candidates;
      _prerequisites = prerequisites;
      Game = game;
    }

    public Player Controller { get { return _prerequisites.Card.Controller; } }

    public IEnumerable<T> Candidates<T>(ControlledBy controlledBy) where T : ITarget
    {
      var candidates = _candidates.HasCost
        ? _candidates.Cost[0]
        : _candidates.Effect[0];

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

    public int TargetCount()
    {
      return _prerequisites.Selector.GetMinEffectTargetCount();
    }
  }
}