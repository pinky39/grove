namespace Grove.Infrastructure
{
  using System;
  using System.Diagnostics;

  public class NullTracker : INotifyChangeTracker
  {    
    [ThreadStatic]
    private static bool _performChecks;

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

    public static void EnableTrackableInitializationChecks()
    {
      _performChecks = true;
    }

    public static void DisableTrackableInitializationChecks()
    {
      _performChecks = false;
    }

    private static void AssertEnabled()
    {
      Debug.Assert(!_performChecks, "Usage of a non initialized Trackable or TrackableList detected!");
    }
  }
}