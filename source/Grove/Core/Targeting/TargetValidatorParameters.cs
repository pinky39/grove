namespace Grove.Core.Targeting
{
  public class TargetValidatorParameters
  {
    public TargetValidatorDelegate TargetSpec = delegate { return true; };
    public ZoneValidatorDelegate ZoneSpec = delegate { return false; };

    public int? MaxCount = 1;
    public string MessageFormat;
    public int MinCount = 1;
    public bool MustBeTargetable;

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