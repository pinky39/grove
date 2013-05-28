namespace Grove.Gameplay.Modifiers
{
  using System;
  using Infrastructure;

  [Serializable]
  public class ControllerSetter : PropertyModifier<Player>
  {
    private readonly Player _value;

    private ControllerSetter() {}

    public ControllerSetter(Player value)
    {
      _value = value;
    }

    public override int Priority { get { return 1; } }

    public override Player Apply(Player before)
    {
      return _value;
    }
  }
}