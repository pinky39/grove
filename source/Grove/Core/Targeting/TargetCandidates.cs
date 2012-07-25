namespace Grove.Core.Targeting
{
  using System.Collections;
  using System.Collections.Generic;
  using Infrastructure;

  [Copyable]
  public class TargetCandidates : IEnumerable<Target>
  {
    private readonly List<Target> _candidates = new List<Target>();

    public IEnumerator<Target> GetEnumerator()
    {
      return _candidates.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public void Add(Target target)
    {
      _candidates.Add(target);
    }
  }
}