namespace Grove.Events
{
  using Effects;

  public interface ICardActivationMessage
  {
    Player Controller { get; }    
    string GetTitle();
  }
}