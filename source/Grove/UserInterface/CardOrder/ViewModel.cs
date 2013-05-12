namespace Grove.UserInterface.CardOrder
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Infrastructure;

  public class ViewModel : ViewModelBase
  {
    private readonly List<OrderedCard> _cards;
    private int _currentIndex = 1;

    public ViewModel(IEnumerable<Card> cards, string title)
    {
      Title = title;
      _cards = cards.Select(x => Bindable.Create<OrderedCard>(x)).ToList();
    }

    public string Title { get; private set; }
    public int[] Ordering { get { return _cards.Select(x => x.Order.Value - 1).ToArray(); } }
    public virtual bool CanAccept { get { return _cards.All(x => x.Order.HasValue); } }
    public IEnumerable<OrderedCard> Cards { get { return _cards; } }

    public void Accept()
    {
      this.Close();
    }

    [Updates("CanAccept")]
    public virtual void Clear()
    {
      foreach (var orderedCard in _cards)
      {
        orderedCard.Order = null;
      }

      _currentIndex = 1;
    }

    [Updates("CanAccept")]
    public virtual void AssignNext(OrderedCard orderedCard)
    {
      if (orderedCard.Order.HasValue)
        return;

      orderedCard.Order = _currentIndex;
      _currentIndex++;
    }    

    public interface IFactory
    {
      ViewModel Create(IEnumerable<Card> cards, string title);
    }
  }
}