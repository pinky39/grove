namespace Grove.Infrastructure
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using log4net;

  public class ChangeTracker : ICopyable
  {
    private static readonly ILog Log = LogManager.GetLogger(typeof (ChangeTracker));
    private readonly Stack<Action> _changeHistory = new Stack<Action>();
    private bool _isEnabled;
    private bool _isLocked;

    public void Copy(object original, CopyService copyService)
    {
      var org = (ChangeTracker) original;
      _isEnabled = org._isEnabled;
    }

    public Snapshot CreateSnapshot()
    {
      Log.Debug("Create snapshot.");

      if (!_isEnabled)
        throw new InvalidOperationException("Tracker is disabled, did you forget to enable it?");

      return new Snapshot(_changeHistory.Count);
    }

    public void Disable()
    {
      if (_changeHistory.Count != 0)
        throw new InvalidOperationException(
          String.Format(
            "Disabling a change tracker with history ({0}) is not allowed. This is a common indication of a leaked copy. The most common cause of this is incorect context use in card definitions (e.g C is used insted of c).",
            _changeHistory.Count));

      _isEnabled = false;
      _changeHistory.Clear();
    }

    public ChangeTracker Enable()
    {
      _isEnabled = true;
      return this;
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

    private void AssertNotLocked()
    {
      if (_isLocked)
      {
        throw new InvalidOperationException(
          "Trying to modify a locked change tracker. This is a common indication of a leaked copy. The most common cause of this is incorect context use in card definitions (e.g C is used insted of c).");
      }
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

      var index = trackableCollection.IndexOf(removed);

      if (index == -1)
        return;

      _changeHistory.Push(() => trackableCollection.InsertWithoutTracking(index, removed));
    }

    public void Restore(Snapshot snapshot)
    {
      Log.Debug("Restore from snapshot.");

      if (!_isEnabled)
        throw new InvalidOperationException("Tracker is disabled, did you forget to enable it?");

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