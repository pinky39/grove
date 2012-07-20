namespace Grove.Core.Ai
{
  using Targeting;

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
    public ITarget Target { get { return Targets.Effect(0); } }
    public Game Game { get; private set; }
    public IPlayer Controller { get { return Source.Controller; } }
    public IPlayer Opponent { get { return Game.Players.GetOpponent(Controller); } }
  }
}