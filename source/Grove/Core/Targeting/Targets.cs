namespace Grove
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using System.Runtime.Serialization;
  using Infrastructure;

  [Copyable, Serializable]
  public class Targets : IEnumerable<ITarget>, IHashable, ISerializable
  {
    private readonly List<ITarget> _costTargets = new List<ITarget>();
    private readonly List<ITarget> _effectTargets = new List<ITarget>();
    public List<int> Distribution;

    public Targets() {}

    public Targets(ITarget effect)
    {
      _effectTargets.Add(effect);
    }

    public Targets(ITarget cost, ITarget effect)
    {
      _costTargets.Add(cost);
      _effectTargets.Add(effect);
    }

    protected Targets(SerializationInfo info, StreamingContext context)
    {
      var ctx = (SerializationContext) context.Context;

      var costTargetsIds = (List<int>) info.GetValue("costTargets", typeof (List<int>));
      var effectTargetsIds = (List<int>) info.GetValue("effectTargets", typeof (List<int>));

      Distribution = (List<int>) info.GetValue("distribution", typeof (List<int>));
      _costTargets.AddRange(costTargetsIds.Select(id => (ITarget) ctx.Recorder.GetObject(id)));
      _effectTargets.AddRange(effectTargetsIds.Select(id => (ITarget) ctx.Recorder.GetObject(id)));
    }    

    public int Count { get { return _effectTargets.Count + _costTargets.Count; } }
    public List<ITarget> Effect { get { return _effectTargets; } }
    public List<ITarget> Cost { get { return _costTargets; } }

    public IEnumerator<ITarget> GetEnumerator()
    {
      foreach (var costTarget in _costTargets)
      {
        yield return costTarget;
      }

      foreach (var effectTarget in _effectTargets)
      {
        yield return effectTarget;
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        HashCalculator.Combine(_effectTargets.Select(calc.Calculate)),
        HashCalculator.Combine(_costTargets.Select(calc.Calculate)));
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      var costTargetsIds = _costTargets.Select(x => x.Id).ToList();
      var effectTargetsIds = _effectTargets.Select(x => x.Id).ToList();

      info.AddValue("costTargets", costTargetsIds);
      info.AddValue("effectTargets", effectTargetsIds);
      info.AddValue("distribution", Distribution);
    }

    public Targets AddCost(ITarget target)
    {
      _costTargets.Add(target);
      return this;
    }

    public Targets AddEffect(ITarget target)
    {
      _effectTargets.Add(target);
      return this;
    }
  }
}