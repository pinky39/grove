namespace Grove.Core.Targeting
{
  using System.Collections.Generic;
  using Ai;
  using Infrastructure;

  public delegate IEnumerable<Target> TargetGeneratorDelegate();

  [Copyable]
  public class TargetSelectors
  {
    private readonly List<TargetSelector> _costSelectors = new List<TargetSelector>();
    private readonly List<TargetSelector> _effectSelectors = new List<TargetSelector>();

    public TargetsFilterDelegate Filter { get; set; }
    public int Count { get { return _costSelectors.Count + _effectSelectors.Count; } }
    public bool HasCost { get { return _costSelectors.Count > 0; } }
    public bool HasEffect { get { return _effectSelectors.Count > 0; } }

    public IEnumerable<TargetSelector> Effect()
    {
      return _effectSelectors;
    }

    public IEnumerable<TargetSelector> Cost()
    {
      return _costSelectors;
    }

    public TargetSelector Effect(int i)
    {
      return i < _effectSelectors.Count ? _effectSelectors[i] : null;
    }

    public TargetSelector Cost(int i)
    {
      return i < _costSelectors.Count ? _costSelectors[i] : null;
    }

    public TargetsCandidates GenerateCandidates(TargetGeneratorDelegate generator)
    {
      var all = new TargetsCandidates();

      foreach (var selector in _costSelectors)
      {
        var candidates = new TargetCandidates();

        foreach (var target in generator())
        {
          if (selector.IsValid(target))
          {
            candidates.Add(target);
          }
        }

        all.AddCostCandidates(candidates);
      }

      foreach (var selector in _effectSelectors)
      {
        var candidates = new TargetCandidates();

        foreach (var target in generator())
        {
          if (selector.IsValid(target))
          {
            candidates.Add(target);
          }
        }

        all.AddEffectCandidates(candidates);
      }

      return all;
    }

    public void AddEffectSelector(TargetSelector selector)
    {
      _effectSelectors.Add(selector);
    }

    public void AddCostSelector(TargetSelector selector)
    {
      _costSelectors.Add(selector);
    }

    public bool AreValidEffectTargets(IList<Target> targets)
    {
      for (var i = 0; i < targets.Count; i++)
      {
        if (_effectSelectors[i].IsValid(targets[i]) == false)
          return false;
      }

      return true;
    }
  }
}