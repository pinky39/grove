namespace Grove.Effects
{
  public class ControllerLoosesLife : Effect
  {
    private readonly int _amount;

    private ControllerLoosesLife() {}

    public ControllerLoosesLife(int amount)
    {
      _amount = amount;
    }

    protected override void ResolveEffect()
    {
      Controller.Life -= _amount;
    }
  }
}