namespace Grove.Core
{
  public class TargetValidatorParameters
  {
    public TargetValidatorParameters(ITarget target, Card source, Game game)
    {
      Target = target;
      Source = source;
      Game = game;
    }

    public ITarget Target { get; private set; }
    public Card Source { get; private set; }
    public Game Game { get; internal set; }
    public Player Controller { get { return Source.Controller; } }
  }
}