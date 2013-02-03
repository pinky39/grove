namespace Grove.Core.Effects
{
  using System;

  public class ControllerGainsLife : Effect
  {
    private readonly Func<Effect, int> _amount;

    private ControllerGainsLife() {}

    public ControllerGainsLife(Func<Effect, int> amount)
    {
      _amount = amount;
    }

    public ControllerGainsLife(int amount) : this(delegate { return amount; }) {}

    protected override void ResolveEffect()
    {
      Controller.Life += _amount(this);
    }
  }
}