namespace Grove.Gameplay.Effects
{
  public class ControllerGainsLifeOpponentLoosesLife : Effect
  {
    private readonly int _amountGained;
    private readonly int _amountLost;

    private ControllerGainsLifeOpponentLoosesLife() {}

    public ControllerGainsLifeOpponentLoosesLife(int amountGained, int amountLost)
    {
      _amountGained = amountGained;
      _amountLost = amountLost;
    }

    protected override void ResolveEffect()
    {
      Controller.Life += _amountGained;
      Controller.Opponent.Life -= _amountLost;
    }
  }
}