namespace Grove.Events
{
  public interface ICardActivationEvent
  {
    Player Controller { get; }
    string GetTitle();
  }
}