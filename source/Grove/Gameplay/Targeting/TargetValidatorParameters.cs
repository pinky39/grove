namespace Grove.Core.Targeting
{
  using System;
  using Modifiers;

  public class TargetValidatorParameters
  {
    public Func<IsValidTargetParam, bool> IsValidTarget = delegate { return true; };
    public Func<IsValidZoneParam, bool> IsValidZone = delegate { return false; };
    public Value MinCount = 1;
    public Value MaxCount = 1;    
    public string Message;    
    public bool MustBeTargetable = true;
    public Func<GetTargetCountParam, Value> GetMinCount;
    public Func<GetTargetCountParam, Value> GetMaxCount;

    public TargetValidatorParameters()
    {
      Is = new TargetSpecs(this);
      In = new ZoneSpecs(this);

      GetMinCount = delegate { return MinCount; };
      GetMaxCount = delegate { return MaxCount; };
    }

    public TargetSpecs Is { get; private set; }
    public ZoneSpecs In { get; private set; }
    public ZoneSpecs On {get { return In; }}
  }
}