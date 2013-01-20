namespace Grove.Core.Targeting
{
  using Costs;
  using Effects;

  public class IsValidTargetParameters : GameObject
  {
    public Effect Effect;
    public Cost Cost;

    public IsValidTargetParameters(ITarget target, Game game)
    {
      Target = target;
      Game = game;
    }

    public ITarget Target { get; private set; }
  }
}