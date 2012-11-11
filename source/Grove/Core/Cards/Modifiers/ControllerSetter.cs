namespace Grove.Core.Details.Cards.Modifiers
{
  using Infrastructure;

  public class ControllerSetter : PropertyModifier<Player>
  {
    private readonly Player _value;

    private ControllerSetter() : base(null)
    {      
    }
    
    public ControllerSetter(Player value, ChangeTracker changeTracker) : base(changeTracker)
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