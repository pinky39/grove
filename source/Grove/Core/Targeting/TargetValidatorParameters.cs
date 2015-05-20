namespace Grove
{
  using System;
  using Modifiers;
      
  public class TargetValidatorParameters
  {
    public Func<GetTargetCountParameters, Value> GetMaxCount;
    public Func<GetTargetCountParameters, Value> GetMinCount;
    public readonly Func<IsValidTargetParameters, bool> IsValidTarget;
    public readonly Func<IsValidZoneParameters, bool> IsValidZone;

    public Value MaxCount = 1;
    public string Message;
    public Value MinCount = 1;        
    
    public bool MustBeTargetable;

    public TargetValidatorParameters(
      Func<IsValidTargetParameters, bool> isValidTarget, 
      Func<IsValidZoneParameters, bool> isValidZone, 
      bool mustBeTargetable = true)
    {      
      GetMinCount = delegate { return MinCount; };
      GetMaxCount = delegate { return MaxCount; };

      IsValidTarget = isValidTarget ?? delegate { return true; };
      IsValidZone = isValidZone ?? delegate { return true; };
      MustBeTargetable = mustBeTargetable;
    }        
  }
}