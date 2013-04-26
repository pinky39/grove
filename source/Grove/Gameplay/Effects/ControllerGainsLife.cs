namespace Grove.Gameplay.Effects
{
  public class ControllerGainsLife : Effect
  {
    private readonly DynParam<int> _amount;

    private ControllerGainsLife() {}

    public ControllerGainsLife(DynParam<int> amount)
    {
      _amount = amount;

      RegisterDynamicParameters(amount);
    }

    protected override void ResolveEffect()
    {
      Controller.Life += _amount.Value;
    }
  }
}