namespace Grove.Infrastructure
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using Castle.DynamicProxy;
  using UserInterface;

  public class Publisher : ICopyable
  {
    private INotifyChangeTracker _changeTracker;
    private EventsCache _eventsCache;
    private Dictionary<Type, TrackableList<object>> _subscribersByEvent = new Dictionary<Type, TrackableList<object>>();

    public Publisher(Assembly assembly = null, INotifyChangeTracker changeTracker = null, string ns = null)
    {
      assembly = assembly ?? Assembly.GetExecutingAssembly();
      _eventsCache = new EventsCache(assembly, ns);
      _changeTracker = changeTracker ?? new ChangeTracker.Guard();
    }

    private Publisher() {}

    public void Copy(object original, CopyService copyService)
    {
      var org = (Publisher) original;
      _changeTracker = copyService.Copy(org._changeTracker);
      _eventsCache = org._eventsCache;
      _subscribersByEvent = new Dictionary<Type, TrackableList<object>>();

      foreach (var subscriber in org._subscribersByEvent)
      {
        var subscribers = subscriber.Value
          .Where(x => IsUiComponent(x) == false)
          .Select(copyService.Copy);

        var trackableSubscribers = new TrackableList<object>(subscribers);
        trackableSubscribers.Initialize(_changeTracker);
        _subscribersByEvent.Add(subscriber.Key, trackableSubscribers);
      }
    }

    public void Publish<TMessage>(TMessage message)
    {
      TrackableList<object> subscribers;
      if (!_subscribersByEvent.TryGetValue(typeof (TMessage), out subscribers))
      {
        return;
      }

      if (subscribers.Count == 0)
        return;
      
      var subscribersCopy = subscribers.ToList();      

      foreach (IReceive<TMessage> subscriber in subscribersCopy)
      {
        subscriber.Receive(message);
      }
    }

    public void Subscribe(object instance)
    {
      var events = _eventsCache.GetEvents(instance);

      if (events == null)
        return;

      foreach (var @event in events)
      {
        TrackableList<object> subscribers;

        if (!_subscribersByEvent.TryGetValue(@event, out subscribers))
        {
          subscribers = new TrackableList<object>().Initialize(_changeTracker);
          _subscribersByEvent[@event] = subscribers;
        }

        subscribers.Add(instance);
      }
    }

    public void Unsubscribe(object instance)
    {
      var events = _eventsCache.GetEvents(instance);

      if (events == null)
        return;

      foreach (var @event in events)
      {
        _subscribersByEvent[@event].Remove(instance);
      }
    }

    private static bool IsUiComponent(object target)
    {
      return ProxyUtil.GetUnproxiedType(target)
        .Namespace.StartsWith(typeof (ViewModelBase).Namespace);
    }

    private class EventsCache
    {
      private readonly Dictionary<Type, List<Type>> _typesToEvents;

      public EventsCache(Assembly assembly, string @namespace)
      {
        var typesAndTheirEvents = assembly.GetTypes()
          .Where(type => (@namespace == null || Equals(type.Namespace, @namespace)) && type.Implements<IReceive>())
          .Select(type => new
            {
              Type = type,
              Events = GetEventsForType(type).ToList()
            });

        _typesToEvents = typesAndTheirEvents.ToDictionary(x => x.Type, x => x.Events);
      }

      public List<Type> GetEvents(object instance)
      {
        List<Type> result;
        _typesToEvents.TryGetValue(ProxyUtil.GetUnproxiedType(instance), out result);
        return result;
      }

      private IEnumerable<Type> GetEventsForType(Type type)
      {
        return type.GetInterfaces()
          .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof (IReceive<>))
          .Select(handlerInterface => handlerInterface.GetGenericArguments()[0]);
      }
    }
  }
}