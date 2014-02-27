namespace Grove
{
  using System.Collections;
  using System.Collections.Generic;
  using Grove.Infrastructure;

  [Copyable]
  public class TargetCandidates : IEnumerable<ITarget>
  {
    private readonly List<ITarget> _candidates = new List<ITarget>();

    public IEnumerator<ITarget> GetEnumerator()
    {
      return _candidates.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public void Add(ITarget target)
    {
      _candidates.Add(target);
    }
  }
}