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

    public void Consume(IManaAmount amount, IManaSource sourceToAvoid)
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

    public bool Has(IManaAmount amount)
    {
      return QuickCheck(amount) ?? SlowCheck(amount);
    }

    private bool? QuickCheck(IManaAmount amount)
    {
      return ColorlessManaCheck(amount) ?? SingleManaCheck(amount);
    }

    private bool? ColorlessManaCheck(IManaAmount amount)
    {
      if (!amount.IsColorless)
        return null;

      var total = amount.Converted;

      foreach (var sourcesWithSameResource in _sources)
      {
        total -= sourcesWithSameResource.Sources[0].GetAvailableMana().Converted;

        if (total <= 0)
          return true;
      }

      return false;
    }

    private bool? SingleManaCheck(IManaAmount amount)
    {
      if (amount.Converted > 1)
        return null;

      foreach (var sourcesWithSameResource in _sources)
      {
        foreach (var source in sourcesWithSameResource.Sources)
        {
          if (source.GetAvailableMana().Has(amount.First))
            return true;
        }
      }

      return false;
    }

    private bool SlowCheck(IManaAmount amount)
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