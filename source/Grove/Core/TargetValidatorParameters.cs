namespace Grove.Core
{
  public class TargetValidatorParameters
  {
    public ITarget Target { get; private set; }
    public Card Source { get; private set; }
    public Game Game { get; internal set; }

    public TargetValidatorParameters(ITarget target, Card source, Game game)
    {
      Target = target;
      Source = source;
      Game = game;
    }
  }
}