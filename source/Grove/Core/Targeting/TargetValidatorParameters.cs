namespace Grove.Core.Targeting
{
  public class TargetValidatorParameters
  {
    public TargetValidatorParameters(Target target, Card source, Game game)
    {
      Target = target;
      Source = source;
      Game = game;
    }

    public Target Target { get; private set; }
    public Card Source { get; private set; }
    public Game Game { get; internal set; }
    public IPlayer Controller { get { return Source.Controller; } }
  }
}