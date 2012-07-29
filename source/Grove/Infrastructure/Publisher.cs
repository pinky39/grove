namespace Grove.Infrastructure
{
  using System.Collections.Generic;
  using System.Linq;
  using Castle.DynamicProxy;

  public class Publisher : ICopyable
  {
    private ChangeTracker _changeTracker;
    private TrackableList<object> _subscribers;

    private Publisher() {}

    public Publisher(ChangeTracker changeTracker)
    {
      _changeTracker = changeTracker;
      _subscribers = new TrackableList<object>(changeTracker);
    }

    public void Copy(object original, CopyService copyService)
    {
      var org = (Publisher) original;

      _changeTracker = copyService.Copy(org._changeTracker);

      var subscribers = org._subscribers
        .Where(x => IsUiComponent(x) == false)
        .Select(copyService.Copy);


      _subscribers = new TrackableList<object>(subscribers, _changeTracker);
    }

    public void Publish<TMessage>(TMessage message)
    {
      NotifySubscribers(message);
    }

    public void Subscribe(object instance)
    {
      if (_subscribers.Contains(instance))
        return;

      _subscribers.Add(instance);
    }

    public bool Unsubscribe(object instance)
    {
      return _subscribers.Remove(instance);
    }

    private static bool IsUiComponent(object target)
    {
      var targetType = ProxyUtil.GetUnproxiedType(target);
      var isUi = targetType.Namespace.StartsWith("Grove.Ui");

      return isUi;
    }

    private void NotifySubscribers<TMessage>(TMessage message)
    {
      var subscribers = _subscribers.ToArray();
      var ordered = new List<IOrderedReceive<TMessage>>();

      foreach (var reference in subscribers)
      {
        var orderedTargets = reference as IOrderedReceive<TMessage>;

        if (orderedTargets != null)
        {
          ordered.Add(orderedTargets);
          continue;
        }

        var target = reference as IReceive<TMessage>;

        if (target != null)
        {
          target.Receive(message);
        }
      }

      foreach (var orderedReceive in ordered.OrderBy(x => x.Order))
      {
        orderedReceive.Receive(message);
      }
    }
  }
}