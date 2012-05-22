namespace Grove.Infrastructure
{
  using System.Collections.Generic;

  public interface ITrackableCollection<T> : ICollection<T>
  {
    void AddWithoutTracking(T item);
    bool RemoveWithoutTracking(T item);
    void InsertWithoutTracking(int index, T item);
    int IndexOf(T item);
  }
}