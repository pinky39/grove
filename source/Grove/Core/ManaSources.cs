namespace Grove.Core
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;

  [Copyable]
  public class ManaSources
  {
    private readonly List<SourcesWithSameResource> _sources;

    public ManaSources() {}

    public ManaSources(IEnumerable<IManaSource> manaSources)
    {
      _sources = manaSources
        .GroupBy(x => x.Resource)
        .Select(x => new SourcesWithSameResource{
          Resource = x.Key,
          Sources = x.OrderBy(y => y.Priority).ToList()
        }).
        ToList();
    }

    public void Add(IEnumerable<IManaSource> manaSources)
    {
      foreach (var manaSource in manaSources)
      {
        var existing = _sources.FirstOrDefault(x => x.Resource == manaSource.Resource);

        if (existing != null)
        {
          existing.Sources.Add(manaSource);
          existing.Sources.Sort((s1, s2) => s1.Priority.CompareTo(s2.Priority));
          continue;
        }

        _sources.Add(new SourcesWithSameResource
        {
          Resource = manaSource.Resource,
          Sources = manaSource.ToEnumerable().ToList()
        });  
      }            
    }

    public void Consume(ManaAmount amount, IManaSource sourceToAvoid)
    {
      foreach (var sources in EnumerateSources())
      {
        if (ManaPayment.Pay(amount, sources, sourceToAvoid))
        {
          break;
        }
      }
    }

    public int GetMaxConvertedMana()
    {
      return _sources.Sum(sourcesWithSameResource =>
        sourcesWithSameResource.Sources.Max(x => x.GetAvailableMana().Converted));
    }

    public bool Has(ManaAmount amount)
    {
      foreach (var sources in EnumerateSources())
      {
        if (ManaPayment.CanPay(amount, sources))
        {
          return true;
        }
      }

      return false;
    }

    public override string ToString()
    {
      return GetMaxConvertedMana().ToString();
    }

    private IEnumerable<List<IManaSource>> EnumerateSources(List<IManaSource> current = null, IEnumerable<SourcesWithSameResource> remaining = null)
    {
      current = current ?? new List<IManaSource>();
      remaining = remaining ?? _sources;

      if (remaining.None())
      {
        yield return current;
        yield break;
      }

      var first = remaining.First();

      foreach (var source in first.Sources)
      {
        current.Add(source);

        foreach (var combination in EnumerateSources(current, remaining.Skip(1)))
        {
          yield return combination;
        }

        current.Remove(source);
      }
    }

    [Copyable]
    public class SourcesWithSameResource
    {
      public object Resource { get; set; }
      public List<IManaSource> Sources { get; set; }
    }
  }
}