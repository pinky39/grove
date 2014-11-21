namespace Grove.Infrastructure
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Runtime.Remoting.Messaging;

  public class ChangeTracker : ICopyable, INotifyChangeTracker
  {
    private readonly Stack<Action> _changeHistory = new Stack<Action>();
    private bool _isEnabled;
    private bool _isLocked;

    public void Copy(object original, CopyService copyService)
    {
      var org = (ChangeTracker) original;
      _isEnabled = org._isEnabled;
    }

    public void NotifyCollectionWillBeCleared<T>(ITrackableCollection<T> trackableCollection)
    {
      AssertNotLocked();

      if (!_isEnabled)
      {
        return;
      }

      var elements = trackableCollection.ToList();

      if (elements.Count == 0)
        return;

      _changeHistory.Push(() =>
        {
          foreach (var element in elements)
          {
            trackableCollection.AddWithoutTracking(element);
          }
        });
    }

    public void NotifyValueAdded<T>(ITrackableCollection<T> trackableCollection, T added)
    {
      AssertNotLocked();

      if (!_isEnabled)
      {
        return;
      }

      _changeHistory.Push(() => trackableCollection.RemoveWithoutTracking(added));
    }

    public void NotifyValueChanged<T>(ITrackableValue<T> trackableValue)
    {
      AssertNotLocked();

      if (!_isEnabled)
      {
        return;
      }

      var value = trackableValue.Value;
      _changeHistory.Push(() => trackableValue.Value = value);
    }

    public void NotifyValueWillBeRemoved<T>(ITrackableCollection<T> trackableCollection, T removed)
    {
      AssertNotLocked();

      if (!_isEnabled)
      {
        return;
      }

      var list = trackableCollection as IList<T>;

      if (list != null)
      {
        var index = list.IndexOf(removed);

        if (index == -1)
          return;

        _changeHistory.Push(() => trackableCollection.InsertWithoutTracking(removed, index));

        return;
      }

      if (trackableCollection.Contains(removed))
      {
        _changeHistory.Push(() => trackableCollection.AddWithoutTracking(removed));
      }
    }

    public Snapshot CreateSnapshot()
    {
      LogFile.Debug("Create snapshot.");
      Asrt.True(_isEnabled, "Tracker is disabled, did you forget to enable it?");

      return new Snapshot(_changeHistory.Count);
    }

    public void Disable()
    {
      Asrt.True(_changeHistory.Count == 0,
        String.Format(
          "Disabling a change tracker with history ({0}) is not allowed. This is a common indication of an incorrect object copy.",
          _changeHistory.Count));

      _isEnabled = false;
      _changeHistory.Clear();
      Guard.Disable();
    }

    public void Enable()
    {
      _isEnabled = true;
      Guard.Enable();
    }

    private void AssertNotLocked()
    {
      Asrt.True(!_isLocked,
        "Trying to modify a locked change tracker. This is a common indication of an incorrect object copy.");
    }

    public void RollbackToSnapshot(Snapshot snapshot)
    {
      LogFile.Debug("Restore from snapshot.");
      Asrt.True(_isEnabled, "Tracker is disabled, did you forget to enable it?");

      while (_changeHistory.Count > snapshot.History)
      {
        var action = _changeHistory.Pop();
        action();
      }
    }

    public void Lock()
    {
      _isLocked = true;
    }

    public void Unlock()
    {
      _isLocked = false;
    }

    public class Guard : INotifyChangeTracker
    {
      private const string EnableKey = "changetracker_enable";

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

        Asrt.Fail("Usage of a non initialized Trackable<> or TrackableList<> detected!");
      }
    }
  }
}