namespace Grove.Core.Ai
{
  public class XCalculatorParameters
  {
    public XCalculatorParameters(Card source, Targets targets, Game game)
    {
      Source = source;
      Targets = targets;
      Game = game;
    }

    public Card Source { get; private set; }
    public Targets Targets { get; private set; }
    public ITarget Target { get { return Targets.Effect; } }
    public Game Game { get; private set; }
    public Player Controller { get { return Source.Controller; } }
    public Player Opponent { get { return Game.Players.GetOpponent(Controller); } }
  }
}