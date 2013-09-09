namespace Grove.Infrastructure
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

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
      AssertEx.True(_isEnabled, "Tracker is disabled, did you forget to enable it?");

      return new Snapshot(_changeHistory.Count);
    }

    public void Disable()
    {
      AssertEx.True(_changeHistory.Count == 0,
        String.Format(
          "Disabling a change tracker with history ({0}) is not allowed. This is a common indication of an incorrect object copy.",
          _changeHistory.Count));

      _isEnabled = false;
      _changeHistory.Clear();
      ChangeTrackerInitializationGuard.Disable();
    }

    public void Enable()
    {
      _isEnabled = true;
      ChangeTrackerInitializationGuard.Enable();
    }

    private void AssertNotLocked()
    {
      AssertEx.True(!_isLocked,
        "Trying to modify a locked change tracker. This is a common indication of an incorrect object copy.");
    }

    public void RollbackToSnapshot(Snapshot snapshot)
    {
      LogFile.Debug("Restore from snapshot.");
      AssertEx.True(_isEnabled, "Tracker is disabled, did you forget to enable it?");

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
  }
}