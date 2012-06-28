namespace Grove.Core
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;

  [Copyable]
  public abstract class TargetBag<T> : IEnumerable<KeyValuePair<string, T>>
  {
    private static readonly HashSet<string> WellKnownTargetNames = new HashSet<string>
      {TargetNames.Effect, TargetNames.Kicker, TargetNames.Cost};

    private readonly Dictionary<string, T> _bag = new Dictionary<string, T>();

    public T Effect { get { return this[TargetNames.Effect]; } set { this[TargetNames.Effect] = value; } }
    public T Kicker { get { return this[TargetNames.Kicker]; } set { this[TargetNames.Kicker] = value; } }
    public T Cost { get { return this[TargetNames.Cost]; } set { this[TargetNames.Cost] = value; } }

    protected IEnumerable<string> Names { get { return _bag.Keys; } }
    protected IEnumerable<T> Values { get { return _bag.Values; } }
    protected int Count { get { return _bag.Count; } }

    public T this[string name]
    {
      get
      {
        T result;
        _bag.TryGetValue(name, out result);
        return result;
      }
      set { _bag[name] = value; }
    }


    public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
    {
      return _bag.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public bool Contains(T value)
    {
      return _bag.ContainsValue(value);
    }

    public void AddAdditionalEffectTargets(TargetBag<T> source)
    {
      var customEffectTargets = source.Where(x => !WellKnownTargetNames.Contains(x.Key));

      foreach (var target in customEffectTargets)
      {
        this[target.Key] = target.Value;
      }
    }
  }
}