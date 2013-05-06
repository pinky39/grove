namespace Grove.Gameplay.Decisions.Results
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;

  [Copyable]
  public class ChosenBlockers : IEnumerable<ChosenBlockers.AttackerBlockerPair>
  {
    private readonly List<AttackerBlockerPair> _pairs = new List<AttackerBlockerPair>();

    public IEnumerator<AttackerBlockerPair> GetEnumerator()
    {
      return _pairs.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public void Add(Card blocker, Card attacker)
    {
      _pairs.Add(new AttackerBlockerPair {Blocker = blocker, Attacker = attacker});
    }

    public bool ContainsBlocker(Card blocker)
    {
      return _pairs.Any(x => x.Blocker == blocker);
    }

    public void Remove(Card blocker)
    {
      var pair = _pairs.First(x => x.Blocker == blocker);
      _pairs.Remove(pair);
    }

    [Copyable]
    public class AttackerBlockerPair
    {
      public Card Attacker { get; set; }
      public Card Blocker { get; set; }
    }
  }
}