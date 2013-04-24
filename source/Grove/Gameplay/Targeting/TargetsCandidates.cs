namespace Grove.Gameplay.Targeting
{
  using System.Collections.Generic;
  using Grove.Infrastructure;

  [Copyable]
  public class TargetsCandidates
  {
    private readonly List<TargetCandidates> _costCandidates = new List<TargetCandidates>();
    private readonly List<TargetCandidates> _effectCandidates = new List<TargetCandidates>();

    public bool HasEffect { get { return _effectCandidates.Count > 0; } }
    public bool HasCost { get { return _costCandidates.Count > 0; } }

    public IList<TargetCandidates> Cost { get { return _costCandidates; } }
    public IList<TargetCandidates> Effect { get { return _effectCandidates; } }

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