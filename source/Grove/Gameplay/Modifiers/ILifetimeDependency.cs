namespace Grove.Gameplay.Modifiers
{
  using Infrastructure;

  public interface ILifetimeDependency
  {
    TrackableEvent EndOfLife { get; set; }
  }
}