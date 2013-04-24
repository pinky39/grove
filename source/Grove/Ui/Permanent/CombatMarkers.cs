namespace Grove.Ui.Permanent
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Gameplay.Card;
  using Infrastructure;

  public class CombatMarkers
  {
    private readonly List<int> _available = Enumerable.Range(1, 100).ToList();
    private readonly Dictionary<Card, int> _used = new Dictionary<Card, int>();

    public int GenerateMarker(Card card)
    {
      var marker = _available.Pop();
      _used.Add(card, marker);
      return marker;
    }

    public int GetMarker(Card card)
    {
      return _used[card];
    }

    public void ReleaseMarker(Card card)
    {
      int marker;
      if (_used.TryGetValue(card, out marker))
      {
        _used.Remove(card);
        _available.Add(marker);
        _available.Sort((e1, e2) => e1.CompareTo(e2));
      }
    }
  }
}