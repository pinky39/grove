namespace Grove.Core.Cards.Modifiers
{
  using Grove.Infrastructure;

  public interface ILifetimeDependency
  {
    TrackableEvent EndOfLife { get; set; }
  }
}