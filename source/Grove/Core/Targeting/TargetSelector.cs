namespace Grove.Core.Targeting
{
  using System.Collections.Generic;
  using System.Linq;
  using Ai;
  using Infrastructure;

  public delegate IEnumerable<ITarget> TargetGeneratorDelegate();

  [Copyable]
  public class TargetSelector
  {
    private readonly List<TargetValidator> _costValidators = new List<TargetValidator>();
    private readonly List<TargetValidator> _effectValidators = new List<TargetValidator>();

    public AiTargetSelectorDelegate SelectAiTargets { get; set; }

    public int Count { get { return _costValidators.Count + _effectValidators.Count; } }
    public bool HasCost { get { return _costValidators.Count > 0; } }
    public bool HasEffect { get { return _effectValidators.Count > 0; } }    
    
    public int GetEffectTargetCount()
    {
      return Effect.Sum(x=> x.MinCount);
    }

    public IList<TargetValidator> Effect { get { return _effectValidators; } }
    public IList<TargetValidator> Cost { get { return _costValidators; } }

    public TargetsCandidates GenerateCandidates(TargetGeneratorDelegate generator)
    {
      var all = new TargetsCandidates();

      foreach (var selector in _costValidators)
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

      foreach (var selector in _effectValidators)
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

    public void AddEffectSelector(TargetValidator validator)
    {
      _effectValidators.Add(validator);
    }

    public void AddCostSelector(TargetValidator validator)
    {
      _costValidators.Add(validator);
    }

    public bool IsValidEffectTarget(ITarget target)
    {
      return _effectValidators[0].IsValid(target);
    }
  }
}