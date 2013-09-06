namespace Grove.Infrastructure
{
  using System.Runtime.Remoting.Messaging;

  public class ChangeTrackerInitializationGuard : INotifyChangeTracker
  {
    private static readonly string EnableKey = "changetracker_enable";

    public void NotifyCollectionWillBeCleared<T>(ITrackableCollection<T> trackableCollection)
    {
      Fail();
    }

    public void NotifyValueAdded<T>(ITrackableCollection<T> trackableCollection, T added)
    {
      Fail();
    }

    public void NotifyValueChanged<T>(ITrackableValue<T> trackableValue)
    {
      Fail();
    }

    public void NotifyValueWillBeRemoved<T>(ITrackableCollection<T> trackableCollection, T removed)
    {
      Fail();
    }

    public static void Enable()
    {
      CallContext.SetData(EnableKey, true);
    }

    public static void Disable()
    {
      CallContext.SetData(EnableKey, false);
    }

    private static void Fail()
    {
      var enable = (bool?) CallContext.GetData(EnableKey);

      if (enable != true)
        return;

      AssertEx.Fail("Usage of a non initialized Trackable<> or TrackableList<> detected!");
    }
  }
}