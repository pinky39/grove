namespace Grove.Effects
{
  public class OpponentRevealsHand : Effect
  {
    private Player _opponent;

    protected override void Initialize()
    {
      _opponent = Controller.Opponent;
    }
    
    protected override void ResolveEffect()
    {
      _opponent.RevealHand();
    }
  }
}