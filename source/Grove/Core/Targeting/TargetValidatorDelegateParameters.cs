namespace Grove.Core.Targeting
{
  using Costs;
  using Effects;

  public class TargetValidatorDelegateParameters : GameObject
  {
    public Effect Effect;
    public Cost Cost;

    public TargetValidatorDelegateParameters(ITarget target, Game game)
    {
      Target = target;
      Game = game;
    }

    public ITarget Target { get; private set; }    
  }
}