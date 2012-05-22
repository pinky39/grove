namespace Grove.Infrastructure
{
  using Core;

  public interface ITrackableValue<T> : IHashable
  {
    T Value { get; set; }
  }
}