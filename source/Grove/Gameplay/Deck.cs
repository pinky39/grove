namespace Grove.Gameplay
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;

  [Serializable]
  public class Deck : IEnumerable<string>
  {
    private readonly List<string> _cards = new List<string>();

    public Deck(IEnumerable<string> cards)
    {
      _cards.AddRange(cards);
    }

    public Deck() {}

    public int? Rating { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
    public int? LimitedCode { get; set; }

    public static Deck CreateUncastable()
    {
      return new Deck(Enumerable.Repeat("Uncastable", 60));
    }

    public IEnumerator<string> GetEnumerator()
    {
      return _cards.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public void AddCard(string name, int count = 1)
    {
      for (var i = 0; i < count; i++)
      {
        _cards.Add(name);
      }
    }

    public bool RemoveCard(string name)
    {
      return _cards.Remove(name);
    }
  }
}