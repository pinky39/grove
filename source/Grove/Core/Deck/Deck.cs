namespace Grove
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  [Serializable]
  public class Deck : IEnumerable<CardInfo>
  {
    private readonly List<CardInfo> _cards = new List<CardInfo>();

    public Deck(IEnumerable<string> cards)
    {
      _cards.AddRange(cards.Select(x => new CardInfo(x)));
    }

    public Deck(IEnumerable<CardInfo> cards)
    {
      _cards.AddRange(cards);
    }

    public Deck() {}

    public int? Rating { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
    public int? LimitedCode { get; set; }
    public int CardCount { get { return _cards.Count; } }

    public CardInfo this[int index] { get { return _cards[index]; } }

    public IEnumerator<CardInfo> GetEnumerator()
    {
      return _cards.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public static Deck CreateUncastable()
    {
      return new Deck(Enumerable.Repeat("Uncastable", 60));
    }

    public override string ToString()
    {
      var sb = new StringBuilder();      
      
      foreach (var cardInfo in _cards)
      {
        sb.AppendLine(cardInfo.ToString());
      }

      return sb.ToString();
    }

    public void AddCard(CardInfo cardInfo, int count = 1)
    {
      for (var i = 0; i < count; i++)
      {
        _cards.Add(cardInfo);
      }
    }

    public bool RemoveCard(CardInfo cardInfo)
    {
      return _cards.Remove(cardInfo);
    }

    public CardInfo Get(string cardName)
    {
      return _cards.FirstOrDefault(x => x.Name == cardName);
    }

    public int GetCount(string cardName)
    {
      return _cards.Count(x => x.Name == cardName);
    }
  }
}