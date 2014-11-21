namespace Grove.Infrastructure
{
  using System;
  using System.Linq;

  public class TrackableEvent : ICopyable
  {
    private INotifyChangeTracker _changeTracker = new ChangeTracker.Guard();
    private TrackableList<Action> _handlers = new TrackableList<Action>();

    public void Copy(object original, CopyService copyService)
    {
      var org = (TrackableEvent) original;

      // create a copy without handlers      
      _changeTracker = copyService.Copy(org._changeTracker);
      _handlers = new TrackableList<Action>();
      _handlers.Initialize(_changeTracker);
    }

    public void Initialize(INotifyChangeTracker changeTracker)
    {
      _changeTracker = changeTracker;
      _handlers.Initialize(changeTracker);
    }

    public void Raise()
    {
      foreach (var eventHandler in _handlers.ToArray())
      {
        eventHandler();
      }
    }

    public void Add(Action eventHandler)
    {
      _handlers.Add(eventHandler);
    }

    public void Remove(Action eventHandler)
    {
      _handlers.Remove(eventHandler);
    }

    public static TrackableEvent operator +(TrackableEvent trackableEvent, Action eventHandler)
    {
      trackableEvent.Add(eventHandler);
      return trackableEvent;
    }

    public static TrackableEvent operator -(TrackableEvent trackableEvent, Action eventHandler)
    {
      trackableEvent.Remove(eventHandler);
      return trackableEvent;
    }
  }

  public class TrackableEvent<TArgs> : ICopyable
  {
    private INotifyChangeTracker _changeTracker = new ChangeTracker.Guard();
    private TrackableList<Action<TArgs>> _handlers = new TrackableList<Action<TArgs>>();

    public void Copy(object original, CopyService copyService)
    {
      var org = (TrackableEvent<TArgs>) original;

      // create a copy without handlers      
      _changeTracker = copyService.Copy(org._changeTracker);
      _handlers = new TrackableList<Action<TArgs>>();
      _handlers.Initialize(_changeTracker);
    }

    public void Initialize(INotifyChangeTracker changeTracker)
    {
      _changeTracker = changeTracker;
      _handlers.Initialize(changeTracker);
    }

    public void Raise(TArgs args)
    {
      foreach (var eventHandler in _handlers.ToArray())
      {
        eventHandler(args);
      }
    }

    public void Add(Action<TArgs> eventHandler)
    {
      _handlers.Add(eventHandler);
    }

    public void Remove(Action<TArgs> eventHandler)
    {
      _handlers.Remove(eventHandler);
    }

    public static TrackableEvent<TArgs> operator +(TrackableEvent<TArgs> trackableEvent, Action<TArgs> eventHandler)
    {
      trackableEvent.Add(eventHandler);
      return trackableEvent;
    }

    public static TrackableEvent<TArgs> operator -(TrackableEvent<TArgs> trackableEvent, Action<TArgs> eventHandler)
    {
      trackableEvent.Remove(eventHandler);
      return trackableEvent;
    }
  }
}