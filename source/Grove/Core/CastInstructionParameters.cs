namespace Grove.Core
{
  using Casting;
  using Costs;

  public class CastInstructionParameters : AbilityOrCastParameters
  {
    public const string KickerDescription = "Cast {0} with kicker.";
    public Cost Cost;    
    public CastingRule Rule;
  }
}