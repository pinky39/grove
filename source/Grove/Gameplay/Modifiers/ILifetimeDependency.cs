namespace Grove.Gameplay.Modifiers
{
  using Grove.Infrastructure;

  public interface ILifetimeDependency
  {
    TrackableEvent EndOfLife { get; set; }
  }
}