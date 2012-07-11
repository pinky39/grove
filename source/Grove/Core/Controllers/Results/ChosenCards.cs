namespace Grove.Core.Controllers.Results
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Targeting;

  public class ChosenCards : IEnumerable<Card>
  {
    private readonly List<Card> _cards = new List<Card>();

    public ChosenCards() {}

    public ChosenCards(IEnumerable<Card> cards)
    {
      _cards.AddRange(cards);
    }

    public IEnumerator<Card> GetEnumerator()
    {
      return _cards.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
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