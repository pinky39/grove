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
    public Player Controller { get { return Source.Controller; } }
    public Card Card {get { return Target.Card(); }}
    public Card Source { get; private set; }
    public Game Game { get; internal set; }    

    public T Trigger<T>()
    {
      return (T) _trigger;
    }
  }
}