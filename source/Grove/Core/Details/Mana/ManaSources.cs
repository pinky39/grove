namespace Grove.Core.Details.Mana
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;

  [Copyable]
  public class ManaSources
  {
    private readonly TrackableList<SourcesWithSameResource> _sources;

    public ManaSources() {}

    public ManaSources(IEnumerable<IManaSource> manaSources, ChangeTracker changeTracker)
    {
      var sourcesWithSameResource = manaSources
        .GroupBy(x => x.Resource)
        .Select(x => new SourcesWithSameResource
          {
            Resource = x.Key,
            Sources = x.OrderBy(y => y.Priority).ToList()
          }).
        ToList();

      _sources = new TrackableList<SourcesWithSameResource>(sourcesWithSameResource, changeTracker);
    }


    public void AddRange(IEnumerable<IManaSource> manaSources)
    {
      foreach (var manaSource in manaSources)
      {
        Add(manaSource);
      }
    }

    public void Add(IManaSource manaSource)
    {
      var existing = _sources.FirstOrDefault(x => x.Resource == manaSource.Resource);

      if (existing != null)
      {
        existing.Sources.Add(manaSource);
        existing.Sources.Sort((s1, s2) => s1.Priority.CompareTo(s2.Priority));
        return;
      }

      _sources.Add(new SourcesWithSameResource
        {
          Resource = manaSource.Resource,
          Sources = manaSource.ToEnumerable().ToList()
        });
    }

    public void Remove(IManaSource manaSource)
    {
      var resource = _sources.FirstOrDefault(x => x.Resource == manaSource.Resource);
      resource.Sources.Remove(manaSource);

      // should remove resource if last source?
      // todo check hierophants bug
    }

    public void Consume(IManaAmount amount, ManaUsage usage, IManaSource sourceToAvoid)
    {
      foreach (var sources in EnumerateSources())
      {
        if (ManaPayment.Pay(amount, sources, usage, sourceToAvoid))
        {
          break;
        }
      }
    }

    public int GetMaxConvertedMana(ManaUsage usage)
    {
      return _sources.Sum(sourcesWithSameResource =>
        sourcesWithSameResource.Sources.Max(x => x.GetAvailableMana(usage).Converted));
    }

    public bool Has(IManaAmount amount, ManaUsage usage)
    {
      return QuickCheck(amount, usage) ?? SlowCheck(amount, usage);
    }

    private bool? QuickCheck(IManaAmount amount, ManaUsage usage)
    {
      if (amount.Converted > GetMaxConvertedMana(usage))
        return false;

      if (amount.IsColorless)
        return true;

      return SingleManaCheck(amount, usage);
    }

    private bool? SingleManaCheck(IManaAmount amount, ManaUsage usage)
    {
      if (amount.Converted > 1)
        return null;

      foreach (var sourcesWithSameResource in _sources)
      {
        foreach (var source in sourcesWithSameResource.Sources)
        {
          if (source.GetAvailableMana(usage).Has(amount.First))
            return true;
        }
      }

      return false;
    }

    private bool SlowCheck(IManaAmount amount, ManaUsage usage)
    {
      foreach (var sources in EnumerateSources())
      {
        if (ManaPayment.CanPay(amount, sources, usage))
        {
          return true;
        }
      }
      return false;
    }

    public override string ToString()
    {
      return GetMaxConvertedMana(ManaUsage.Any).ToString();
    }

    private IEnumerable<List<IManaSource>> EnumerateSources(List<IManaSource> current = null, int currentIndex = 0)
    {
      current = current ?? new List<IManaSource>();

      if (currentIndex == _sources.Count)
      {
        yield return current;
        yield break;
      }

      var first = _sources[currentIndex];

      foreach (var source in first.Sources)
      {
        current.Add(source);

        foreach (var combination in EnumerateSources(current, currentIndex + 1))
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