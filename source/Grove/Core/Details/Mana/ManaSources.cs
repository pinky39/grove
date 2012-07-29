namespace Grove.Core.Details.Mana
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
        .Select(x => new SourcesWithSameResource
          {
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

    public void Consume(IManaAmount amount, ManaUsage usage, IManaSource sourceToAvoid)
    {
      foreach (var sources in EnumerateSources())
      {
        if (ManaPayment.Pay(amount, sources,  usage, sourceToAvoid))
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
      return ColorlessManaCheck(amount, usage) ?? SingleManaCheck(amount, usage);
    }

    private bool? ColorlessManaCheck(IManaAmount amount, ManaUsage usage)
    {
      if (!amount.IsColorless)
        return null;

      var total = amount.Converted;

      foreach (var sourcesWithSameResource in _sources)
      {
        total -= sourcesWithSameResource.Sources[0].GetAvailableMana(usage).Converted;

        if (total <= 0)
          return true;
      }

      return false;
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