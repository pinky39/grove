namespace Grove.Core.Details.Mana
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;

  public class ManaPayment
  {
    private readonly int _available;

    private readonly Dictionary<ManaColors, List<ManaBucket>> _bucketsHolders =
      new Dictionary<ManaColors, List<ManaBucket>>();

    private readonly List<ManaBucket> _items = new List<ManaBucket>();

    private ManaPayment(List<ScoredSource> available)
    {
      FillTheBucket(available);

      _available = available.Sum(x => x.Amount.Converted);
    }

    private ManaPayment(IManaAmount available)
    {
      FillTheBucket(available);

      _available = available.Converted;
    }

    public static bool CanPay(IManaAmount cost, IEnumerable<IManaSource> manaSources)
    {
      var available = new AggregateManaAmount(manaSources.Select(x => x.GetAvailableMana()));

      if (cost.Converted > available.Converted)
        return false;

      return new ManaPayment(available).CanAssign(cost);
    }

    public static void Pay(IManaAmount cost, IManaSource manaSource)
    {
      Pay(cost, manaSource.ToEnumerable());
    }

    public static bool Pay(IManaAmount cost, IEnumerable<IManaSource> manaSources, IManaSource sourceToAvoid = null)
    {
      var scoredSources = manaSources
        .Select(source =>
          {
            var availableMana = source.GetAvailableMana();

            var manasource = new ScoredSource(
              source,
              availableMana,
              ScoreSource(source, sourceToAvoid, availableMana));

            return manasource;
          })
        .Where(x => !x.Amount.IsZero())
        .OrderBy(x => x.Score)
        .ToList();


      var payment = new ManaPayment(scoredSources).Assign(cost);

      if (payment == null)
        return false;

      foreach (var pair in payment)
      {
        pair.Key.Consume(new PrimitiveManaAmount(pair.Value));
      }

      return true;
    }

    private static int ScoreSource(IManaSource source, IManaSource sourceToAvoid, IManaAmount availableMana)
    {
      var score = source.Priority;

      if (sourceToAvoid == source)
        score += 100000;

      return score + availableMana.MaxRank();
    }

    private Dictionary<IManaSource, List<ManaUnit>> Assign(IManaAmount amount)
    {
      if (amount.Converted > _available)
      {
        return null;
      }

      if (!TryAssignColored(amount))
        return null;

      AssignColorless(amount);

      return GetAssignments();
    }

    private void AssignColorless(IManaAmount amount)
    {
      foreach (var bucket in _items.Where(x => !x.IsFilled).Take(amount.ColorlessCount()))
      {
        bucket.Fill(ManaColors.Colorless);
      }
    }

    private bool CanAssign(IManaAmount amount)
    {
      if (amount.Converted > _available)
      {
        return false;
      }

      return TryAssignColored(amount);
    }

    private void CreateBucketHolders()
    {
      _bucketsHolders.Add(ManaColors.White, new List<ManaBucket>());
      _bucketsHolders.Add(ManaColors.Blue, new List<ManaBucket>());
      _bucketsHolders.Add(ManaColors.Black, new List<ManaBucket>());
      _bucketsHolders.Add(ManaColors.Red, new List<ManaBucket>());
      _bucketsHolders.Add(ManaColors.Green, new List<ManaBucket>());
    }

    private void FillTheBucket(IManaAmount available)
    {
      CreateBucketHolders();

      foreach (var mana in available)
      {
        var bucket = new ManaBucket
          {
            Mana = mana
          };

        foreach (var color in mana.EnumerateColors())
        {
          _bucketsHolders[color].Add(bucket);
        }

        _items.Add(bucket);
      }
    }

    private void FillTheBucket(List<ScoredSource> sources)
    {
      CreateBucketHolders();

      var scoredSources = sources
        .SelectMany(x => x.Amount.Select(
          mana => new
            {
              Mana = mana,
              x.Score,
              x.Source
            }));


      foreach (var scoredSource in scoredSources)
      {
        var bucket = new ManaBucket
          {
            Source = scoredSource.Source,
            Mana = scoredSource.Mana
          };

        foreach (var color in scoredSource.Mana.EnumerateColors())
        {
          _bucketsHolders[color].Add(bucket);
        }
        _items.Add(bucket);
      }
    }

    private Dictionary<IManaSource, List<ManaUnit>> GetAssignments()
    {
      var assignments = new Dictionary<IManaSource, List<ManaUnit>>();

      foreach (var item in _items)
      {
        if (item.IsFilled)
        {
          List<ManaUnit> manaList;

          if (!assignments.TryGetValue(item.Source, out manaList))
          {
            manaList = new List<ManaUnit>();
            assignments[item.Source] = manaList;
          }

          manaList.Add(item.Mana);
        }
      }

      return assignments;
    }

    private bool TryAssignColored(IManaAmount amount)
    {
      foreach (var mana in amount.Where(mana => mana.IsColored))
      {
        foreach (var color in mana.EnumerateColors())
        {
          var success = TryToFindBucketForColor(color);

          if (success)
            break;

          return false;
        }
      }

      return true;
    }

    private bool TryToFindBucketForColor(ManaColors color, HashSet<ManaColors> forbidden = null)
    {
      var emptyBucket = _bucketsHolders[color].FirstOrDefault(x => !x.IsFilled);
      forbidden = forbidden ?? new HashSet<ManaColors>();

      if (emptyBucket == null)
      {
        foreach (var item in _bucketsHolders[color])
        {
          if (item.FilledWithColor != color && !forbidden.Contains(item.FilledWithColor))
          {
            // add current to forbidden list to avoid cycles
            forbidden.Add(color);
            var success = TryToFindBucketForColor(item.FilledWithColor, forbidden);
            if (success)
            {
              item.Fill(color);
              return true;
            }
          }
        }
        return false;
      }

      emptyBucket.Fill(color);
      return true;
    }

    private class ManaBucket
    {
      public ManaColors FilledWithColor { get; private set; }

      public bool IsFilled { get { return FilledWithColor != ManaColors.None; } }

      public ManaUnit Mana { get; set; }
      public IManaSource Source { get; set; }

      public void Fill(ManaColors color)
      {
        FilledWithColor = color;
      }
    }

    private class ScoredSource
    {
      public ScoredSource(IManaSource source, IManaAmount amount, int score = 0)
      {
        Source = source;
        Amount = amount;
        Score = score;
      }

      public IManaAmount Amount { get; private set; }
      public int Score { get; private set; }
      public IManaSource Source { get; private set; }
    }
  }
}