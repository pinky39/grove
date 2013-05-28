namespace Grove.Infrastructure
{
  using System;
  using System.Diagnostics;

  [Serializable]
  public class NullTracker : INotifyChangeTracker
  {
    private static bool _enableChecks;

    public void NotifyCollectionWillBeCleared<T>(ITrackableCollection<T> trackableCollection)
    {
      AssertEnabled();
    }

    public void NotifyValueAdded<T>(ITrackableCollection<T> trackableCollection, T added)
    {
      AssertEnabled();
    }

    public void NotifyValueChanged<T>(ITrackableValue<T> trackableValue)
    {
      AssertEnabled();
    }

    public void NotifyValueWillBeRemoved<T>(ITrackableCollection<T> trackableCollection, T removed)
    {
      AssertEnabled();
    }

    public static void EnableChangeTrackerChecks()
    {
      _enableChecks = true;
    }

    private static void AssertEnabled()
    {
      Debug.Assert(!_enableChecks, "Usage of a non initialized Trackable or TrackableList detected!");
    }
  }
}