namespace Grove.Events
{
  public interface ICardActivationMessage
  {
    Player Controller { get; }
    string GetTitle();
  }
}