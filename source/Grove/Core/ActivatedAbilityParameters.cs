namespace Grove.Core
{
  using Costs;
  using Zones;

  public class ActivatedAbilityParameters : AbilityParameters
  {
    public bool ActivateAsSorcery;
    public Zone ActivationZone;
    public Cost Cost;
  }
}