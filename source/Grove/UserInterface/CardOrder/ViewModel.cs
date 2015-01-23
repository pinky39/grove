namespace Grove.UserInterface.CardOrder
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;

  public class ViewModel : ViewModelBase
  {
    private List<OrderedObject> _objects;    
    private int _currentIndex = 1;
    private int _multiplicator = 1;
    private bool _orderPair;

    public ViewModel(IEnumerable<object> objects, string title, bool orderPair = false)
    {
      Title = title;
      _orderPair = orderPair;
      _objects = objects.Select(x => Bindable.Create<OrderedObject>(x)).ToList();
    }

    public string Title { get; private set; }

    public int[] Ordering
    {
      get
      {
        return _objects.Select(x => _multiplicator > 0 ? x.Order.Value - 1 : x.Order.Value).ToArray();
      }
    }

    public virtual bool CanAccept
    {
      get
      {
        return _orderPair || _objects.All(x => x.Order.HasValue);
      }
    }

    public IEnumerable<OrderedObject> Objects
    {
      get
      {
        return _multiplicator > 0 ? _objects : _objects.Where(x => !x.Order.HasValue).ToList(); 
      }
    }

    [Updates("Objects")]
    public virtual void Accept()
    {
      if (_orderPair)
      {
        _orderPair = false;
        _currentIndex = 1;
        _multiplicator = -1;

        return;
      }

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

      orderedCard.Order = _multiplicator * _currentIndex;
      
      _currentIndex++;
    }   

    public interface IFactory
    {
      ViewModel Create(IEnumerable<object> objects, string title, bool orderPair = false);
    }
  }
}