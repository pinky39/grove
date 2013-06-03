namespace Grove.Gameplay.Decisions.Results
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using System.Runtime.Serialization;
  using Persistance;
  using Targeting;

  [Serializable]
  public class ChosenCards : IEnumerable<Card>, ISerializable
  {
    private readonly List<Card> _cards = new List<Card>();

    public ChosenCards() {}

    public ChosenCards(IEnumerable<Card> cards)
    {
      _cards.AddRange(cards);
    }

    public ChosenCards(SerializationInfo info, StreamingContext context)
    {
      var ctx = (SerializationContext) context.Context;
      var cardIds = (List<int>) info.GetValue("cards", typeof (List<int>));

      _cards.AddRange(cardIds.Select(x => (Card) ctx.Recorder.GetObject(x)));
    }

    public static ChosenCards None { get { return new ChosenCards(); } }
    public int Count { get { return _cards.Count; } }

    public Card this[int index] { get { return _cards[index]; } }

    public IEnumerator<Card> GetEnumerator()
    {
      return _cards.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      var cardIds = _cards.Select(x => x.Id).ToList();
      info.AddValue("cards", cardIds);
    }

    public void Add(Card card)
    {
      _cards.Add(card);
    }

    public static implicit operator ChosenCards(List<ITarget> cards)
    {
      return new ChosenCards(cards.Select(x => x.Card()));
    }

    public static implicit operator ChosenCards(List<Card> cards)
    {
      return new ChosenCards(cards);
    }
  }
}