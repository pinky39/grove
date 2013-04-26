namespace Grove.Gameplay.Targeting
{
  using System;
  using Modifiers;

  public class TargetValidatorParameters
  {
    public Func<GetTargetCountParam, Value> GetMaxCount;
    public Func<GetTargetCountParam, Value> GetMinCount;
    public Func<IsValidTargetParam, bool> IsValidTarget = delegate { return true; };
    public Func<IsValidZoneParam, bool> IsValidZone = delegate { return false; };
    public Value MaxCount = 1;
    public string Message;
    public Value MinCount = 1;
    public bool MustBeTargetable = true;

    public TargetValidatorParameters()
    {
      Is = new TargetSpecs(this);
      In = new ZoneSpecs(this);

      GetMinCount = delegate { return MinCount; };
      GetMaxCount = delegate { return MaxCount; };
    }

    public TargetSpecs Is { get; private set; }
    public ZoneSpecs In { get; private set; }
    public ZoneSpecs On { get { return In; } }
  }
}