namespace Grove.Infrastructure
{
  public interface INotifyChangeTracker
  {
    void NotifyCollectionWillBeCleared<T>(ITrackableCollection<T> trackableCollection);
    void NotifyValueAdded<T>(ITrackableCollection<T> trackableCollection, T added);
    void NotifyValueChanged<T>(ITrackableValue<T> trackableValue);
    void NotifyValueWillBeRemoved<T>(ITrackableCollection<T> trackableCollection, T removed);
  }
}