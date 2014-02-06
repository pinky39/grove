namespace Grove.Gameplay.Messages
{
  public interface ICardActivationMessage
  {
    Player Controller { get; }
    string GetTitle();
  }
}