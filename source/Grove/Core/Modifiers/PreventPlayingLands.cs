namespace Grove.Modifiers
{
  public class PreventPlayingLands : Modifier, IPlayerModifier
  {
    private LandLimit _landLimit;
    private readonly IntegerSetter _integerSetter = new IntegerSetter(-1000);

    public override void Apply(LandLimit landLimit)
    {
      _landLimit = landLimit;
      _integerSetter.Initialize(ChangeTracker);
      _landLimit.AddModifier(_integerSetter);
    }
    
    protected override void Unapply()
    {
      _landLimit.RemoveModifier(_integerSetter);
    }
  }
}