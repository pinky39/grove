namespace Grove.Core.Targeting
{
  public class TargetValidatorDelegateParameters
  {
    public Game Game;
    public ITarget Target;
    public Card OwningCard;

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