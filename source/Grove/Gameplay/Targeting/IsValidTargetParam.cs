namespace Grove.Gameplay.Targeting
{
  public class IsValidTargetParam
  {
    public Player Controller;
    public Game Game;
    public Card OwningCard;
    public ITarget Target;

    private object _message;

    public void SetTriggerMessage(object message)
    {
      _message = message;
    }

    public T TriggerMessage<T>()
    {
      return (T) _message;
    }
  }
}