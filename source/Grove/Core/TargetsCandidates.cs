namespace Grove.Core
{
  using System.Collections.Generic;
  using Infrastructure;

  [Copyable]
  public class TargetsCandidates
  {
    private readonly List<TargetCandidates> _costCandidates = new List<TargetCandidates>();
    private readonly List<TargetCandidates> _effectCandidates = new List<TargetCandidates>();

    public TargetCandidates Cost(int i)
    {
      return i < _costCandidates.Count ? _costCandidates[i] : null;
    }

    public TargetCandidates Effect(int i)
    {
      return i < _effectCandidates.Count ? _effectCandidates[i] : null;
    }

    public void AddCostCandidates(TargetCandidates candidates)
    {
      _costCandidates.Add(candidates);
    }

    public void AddEffectCandidates(TargetCandidates candidates)
    {
      _effectCandidates.Add(candidates);
    }
  }
}