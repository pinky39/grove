namespace Grove.UserInterface.CardOrder
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;

  public class ViewModel : ViewModelBase
  {
    private readonly List<OrderedObject> _objects;
    private int _currentIndex = 1;

    public ViewModel(IEnumerable<object> objects, string title)
    {
      Title = title;
      _objects = objects.Select(x => Bindable.Create<OrderedObject>(x)).ToList();
    }

    public string Title { get; private set; }
    public int[] Ordering { get { return _objects.Select(x => x.Order.Value - 1).ToArray(); } }
    public virtual bool CanAccept { get { return _objects.All(x => x.Order.HasValue); } }
    public IEnumerable<OrderedObject> Objects { get { return _objects; } }

    public void Accept()
    {
      this.Close();
    }

    [Updates("CanAccept")]
    public virtual void Clear()
    {
      foreach (var orderedCard in _objects)
      {
        orderedCard.Order = null;
      }

      _currentIndex = 1;
    }

    [Updates("CanAccept")]
    public virtual void AssignNext(OrderedObject orderedCard)
    {
      if (orderedCard.Order.HasValue)
        return;

      orderedCard.Order = _currentIndex;
      _currentIndex++;
    }   

    public interface IFactory
    {
      ViewModel Create(IEnumerable<object> objects, string title);
    }
  }
}