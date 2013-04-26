namespace Grove.Tests.Unit
{
  using System;
  using Grove.Infrastructure;
  using Xunit;

  public class TrackableEventFacts
  {
    [Fact]
    public void Register()
    {
      var tevent = CreateEvent();
      var count = 0;
      var snapshot = _changeTracker.CreateSnapshot();

      tevent += delegate { count++; };
      tevent.Raise();

      _changeTracker.Restore(snapshot);

      tevent.Raise();

      Assert.Equal(1, count);
    }

    [Fact]
    public void Unregister()
    {
      var tevent = CreateEvent();
      var count = 0;
      EventHandler handler = delegate { count++; };
      tevent += handler;

      var snapshot = _changeTracker.CreateSnapshot();
      tevent -= handler;
      tevent.Raise();
      Assert.Equal(0, count);

      _changeTracker.Restore(snapshot);
      tevent.Raise();
      Assert.Equal(1, count);
    }

    private readonly ChangeTracker _changeTracker = new ChangeTracker().Enable();

    private TrackableEvent CreateEvent()
    {
      return new TrackableEvent(this).Initialize(_changeTracker);
    }
  }
}