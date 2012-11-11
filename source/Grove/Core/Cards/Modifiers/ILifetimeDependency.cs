namespace Grove.Core.Details.Cards.Modifiers
{
  using Infrastructure;

  public interface ILifetimeDependency
  {
    TrackableEvent EndOfLife { get; set; }
  }
}