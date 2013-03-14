namespace Grove.Core.Targeting
{
  using Modifiers;

  public class TargetValidatorParameters
  {
    public TargetValidatorDelegate TargetSpec = delegate { return true; };
    public ZoneValidatorDelegate ZoneSpec = delegate { return false; };

    public Value MaxCount = 1;
    public string Text;
    public Value MinCount = 1;
    public bool MustBeTargetable = true;

    public TargetValidatorParameters()
    {
      Is = new TargetSpecs(this);
      In = new ZoneSpecs(this);      
    }

    public TargetSpecs Is { get; private set; }
    public ZoneSpecs In { get; private set; }
    public ZoneSpecs On {get { return In; }}
  }
}