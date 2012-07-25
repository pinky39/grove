namespace Grove.Core.Targeting
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;

  [Copyable]
  public class Targets : IEquatable<Targets>
  {
    private readonly List<Target> _costTargets = new List<Target>();
    private readonly List<Target> _effectTargets = new List<Target>();
    public int Count { get { return _effectTargets.Count + _costTargets.Count; } }

    public bool Any { get { return Count > 0; } }

    public bool None { get { return Count == 0; } }

    public bool Equals(Targets other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;

      return other.GetHashCode() == GetHashCode();
    }

    public IEnumerable<Target> Effect()
    {
      return _effectTargets;
    }

    public Target Effect(int i)
    {
      return i < _effectTargets.Count ? _effectTargets[i] : null;
    }

    public IEnumerable<Target> Cost()
    {
      return _costTargets;
    }

    public Target Cost(int i)
    {
      return i < _costTargets.Count ? _costTargets[i] : null;
    }

    public Targets AddCost(Target target)
    {
      _costTargets.Add(target);
      return this;
    }

    public Targets AddEffect(Target target)
    {
      _effectTargets.Add(target);
      return this;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != typeof (Targets)) return false;
      return Equals((Targets) obj);
    }

    public override int GetHashCode()
    {
      return HashCalculator.Combine(
        _effectTargets.Select(x => x.GetHashCode())
          .Concat(_costTargets.Select(x => x.GetHashCode()))
        );
    }
  }
}