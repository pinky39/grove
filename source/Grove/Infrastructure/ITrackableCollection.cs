namespace Grove.Infrastructure
{
  using System.Collections.Generic;

  public interface ITrackableCollection<T> : IEnumerable<T>
  {
    void AddWithoutTracking(T item);
    bool RemoveWithoutTracking(T item);
    void InsertWithoutTracking(T item, int index);
  }
}