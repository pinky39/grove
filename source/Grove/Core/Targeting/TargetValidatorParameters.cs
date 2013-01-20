namespace Grove.Core.Targeting
{
  public class TargetValidatorParameters
  {
    public IsValidTarget IsValidTarget = delegate { return true; };
    public IsValidZone IsValidZone = delegate { return true; };
    public int? MaxCount = 1;
    public int MinCount = 1;
    public string MessageFormat;
    public bool MustBeTargetable;
  }
}