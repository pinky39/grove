namespace Grove.Ai
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Gameplay.Card;
  using Gameplay.Card.Abilities;
  using Gameplay.Targeting;

  public class ActivationContext
  {
    private readonly List<TargetsCombination> _targets = new List<TargetsCombination>();
    public bool CanCancel = true;
    public bool CancelActivation;
    public int Repeat = 1;
    public object TriggerMessage;
    public int? X;

    public ActivationContext(Card card, TargetSelector selector)
    {
      Card = card;
      Selector = selector;
    }

    public ActivationContext(ActivationPrerequisites prerequisites)
    {
      Card = prerequisites.Card;
      MaxX = prerequisites.MaxX;
      Selector = prerequisites.Selector;
      DistributeAmount = prerequisites.DistributeAmount;
      MaxRepetitions = prerequisites.MaxRepetitions;
    }

    public int MaxRepetitions { get; private set; }

    public Card Card { get; private set; }
    public int? MaxX { get; private set; }
    public TargetSelector Selector { get; private set; }
    public int DistributeAmount { get; private set; }

    public bool HasTargets { get { return _targets.Count > 0; } }

    public void SetPossibleTargets(IEnumerable<Targets> targetsCombinations)
    {
      _targets.AddRange(targetsCombinations.Select(x => new TargetsCombination
        {
          Targets = x,
          X = X,
        }));
    }

    public IEnumerable<TargetsCombination> TargetsCombinations()
    {
      return _targets;
    }

    public void RemoveTargetCombination(TargetsCombination targetsCombination)
    {
      _targets.Remove(targetsCombination);
    }

    public class TargetsCombination
    {
      public Targets Targets;
      public int? X;
    }
  }
}