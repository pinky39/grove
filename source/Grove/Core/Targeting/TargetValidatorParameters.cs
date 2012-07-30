namespace Grove.Core.Targeting
{
  public class TargetValidatorParameters
  {
    private readonly object _trigger;

    public TargetValidatorParameters(ITarget target, Card source,  object trigger, Game game)
    {
      _trigger = trigger;
      Target = target;
      Source = source;
      Game = game;
    }

    public ITarget Target { get; private set; }
    public Card Source { get; private set; }
    public Game Game { get; internal set; }
    public Player Controller { get { return Source.Controller; } }

    public T Trigger<T>()
    {
      return (T) _trigger;
    }
  }
}