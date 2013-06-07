namespace Grove.Infrastructure
{
  using System;
  using System.Linq;

  public class TrackableEvent : ICopyable
  {
    private INotifyChangeTracker _changeTracker;
    private TrackableList<EventHandler> _handlers = new TrackableList<EventHandler>();
    private object _sender;

    private TrackableEvent() {}

    public TrackableEvent(object sender)
    {
      _sender = sender;
    }

    public void Copy(object original, CopyService copyService)
    {
      var org = (TrackableEvent) original;

      // create a copy without handlers
      _sender = copyService.Copy(org._sender);
      _changeTracker = copyService.Copy(org._changeTracker);
      _handlers = new TrackableList<EventHandler>();
      _handlers.Initialize(_changeTracker);
    }

    public TrackableEvent Initialize(INotifyChangeTracker changeTracker)
    {
      _changeTracker = changeTracker;
      _handlers.Initialize(changeTracker);
      return this;
    }

    public void Raise()
    {
      foreach (var eventHandler in _handlers.ToArray())
      {
        eventHandler(_sender, EventArgs.Empty);
      }
    }

    public void Add(EventHandler eventHandler)
    {
      _handlers.Add(eventHandler);
    }

    public void Remove(EventHandler eventHandler)
    {
      _handlers.Remove(eventHandler);
    }

    public static TrackableEvent operator +(TrackableEvent trackableEvent, EventHandler eventHandler)
    {
      trackableEvent.Add(eventHandler);
      return trackableEvent;
    }

    public static TrackableEvent operator -(TrackableEvent trackableEvent, EventHandler eventHandler)
    {
      trackableEvent.Remove(eventHandler);
      return trackableEvent;
    }
  }
}